# BDD toolkit for *.net*

## About

**BDD toolkit** is a lightweight .NET library for implementing BDD scenarios in pure C#. No enforcement of regular expressions. No strict conventions. Instead - close integration with C# syntax and maximum usage of its expressiveness. Thanks to that you can get instantly productive and  focus on the domain you test.

## Why BDD toolkit?

[Here you can read more about our approach.](#our-approach)

- Fluent syntax to write tests in Given - When - Then style
- Correct exceptions handling -  all of assertions are checked even when an exception was thrown ([more info here](#testing-rules)) 
- Can be used with any test framework (xUnit, NUnit, MSTest)
- Generation of readable test scenarios directly from tests code (no semicolons, parentheses - only **pure text** - [more info here](#generating-docs))

## Quick Start

### Installation

Install [BDD-toolkit-dotnet NuGet package](https://www.nuget.org/packages/BDD-toolkit-dotnet/) in your test projects.

### Add class for tests

The first step is to create a test class for your component, feature or any other unit. In our examples it would be `PrePaidAccountTests`

Next, you need to create a class to call your domain code when running scenario steps. It could be a private nested class inside the test class created before. In our example it will be named `Context` (but it can have any other name)

```c#
public class PrePaidAccountTests
{
    [Fact]
    public void CanPayUpToSumOfAmountAvailableAndDebtLimit()
    {
    	//...
    }
   
    private class Context
    {
        //...
    }
}
```

### Adding a first test

**BDD** is a true **TDD**. We support and recommend writing test scenarios in advance, together with business people, even before the first line of code is written. More! Very often it is good to know them even before we have any top-level design of code!

**BDD toolkit** fully supports such **TDD**-oriented approach allowing you to write tests even when no implementation is present. To start doing that just type `Bdd` and follow the fluent API like in the following example:  

```c#
[Fact]
public void CanPayUpToSumOfAmountAvailableAndDebtLimit() => Bdd.Scenario<Context>()
    .Given(c => c.AmountAvailableWas(10, Currency.PLN))
    .And(c => c.DebtLimitWas(100, Currency.PLN))
    .And(c => c.DebtWas(0, Currency.PLN))
    .When(c => c.AccountIsCharged(20, Currency.PLN))
    .Then(c => c.AmountAvailableIs(0, Currency.PLN))
    .And(c => c.DebtIs(10, Currency.PLN))
    .Test();
```

Method names should represent steps from the previously created scenarios. The easiest way to define them is to: 

1. First write your new method name in your test scenario as if it existed ( it will be marked in red, but you'll get the feeling of writing in natural language). 
1. Use your IDE to generate the missing methods. They will be created in the class named `Context` passed to the method `Bdd.Scenario<Context>()`

Do we need implementation of these methods? Not at this stage!

```c#
private class Context
{
    public void AvailableAmountWas(decimal value, Currency currency)
    {
       throw new NotImplementedException();
    }

    public void DebtLimitWas(decimal value, Currency currency)
    {
        throw new NotImplementedException();
    }

    //...
}
```

### Add more tests

For now there is no implementation for the defined scenarios so your tests *will* fail. However we have defined our business requirements *directly in code* which is already a big win. Now it is enough to follow the **TDD** approach i.e. implement the model and connect it to scenario by completing the methods inside the `Context` class. 

Thanks to that we get a form of **Living Documentation**. Tests written this way should be *always* up-to date with the current business requirements.

### Add domain model's types and methods

The next step is preparing the actual design of our  **domain model**. We can start with types and methods' headers. In our case we will need a class called `PrePaidAccount`. Its model could look like that: 

```c#
public class PrePaidAccount
{
    //...
    
    public static PrePaidAccount Restore(Snapshot snapshot) => new PrePaidAccount(
        snapshot.AmountAvailable,
        snapshot.DebtLimit,
        snapshot.Debt);

    private PrePaidAccount(Money amountAvailable, Money debtLimit, Money debt)
    {
        _amountAvailable = amountAvailable;
        _debtLimit = debtLimit;
        _debt = debt;
    }   
        
    public void Charge(Money amount)
    {
        throw new NotImplementedException();
    }
    
    //...
        
    public Snapshot GetSnapshot() => new Snapshot(_amountAvailable, _debtLimit, _debt);
        
    public readonly struct Snapshot
    {
        public Money AmountAvailable { get; }
        public Money DebtLimit { get; }
        public Money Debt { get; }

        public Snapshot(Money amountAvailable, Money debtLimit, Money debt)
        {
            AmountAvailable = amountAvailable;
            DebtLimit = debtLimit;
            Debt = debt;
        }
    }
}
```

### Connecting the tests and the domain model

A prepared **domain model** can be used in test in the following way:

```c#
private class Context
{
    private Money _amountAvailable;
    private Money _debtLimit;
    private Money _debt;
    private PrePaidAccount _account;
    
    public void AmountAvailableWas(decimal value, Currency currency) => 
    	_amountAvailable = Money.Of(value, currency);

    public void DebtLimitWas(decimal value, Currency currency) => 
    	_debtLimit = Money.Of(value, currency);

    public void DebtWas(decimal value, Currency currency) => 
    	_debt = Money.Of(value, currency);

    public void AccountIsCharged(decimal value, Currency currency)
    {
        _account = PrePaidAccount.Restore(
        	new PrePaidAccount.Snapshot(_amountAvailable, _debtLimit, _debt));
        _account.Charge(Money.Of(value, currency));
    }

    public void AmountAvailableIs(decimal value, Currency currency) => 
    	_account.GetSnapshot().AmountAvailable.Should().Be(Money.Of(value, currency));

    public void DebtIs(decimal value, Currency currency) => 
    	_account.GetSnapshot().Debt.Should().Be(Money.Of(value, currency));
    
    //...
}
```

The test  won't pass because in the **domain model** we still have only headers and types without the actual implementation. Time to fix that. 

### Implementing behaviors inside the domain model

The final implementation of `PrePaidAccount` could look like that:

```c#
public class PrePaidAccount
{
    private readonly Money _amountAvailable;
    private readonly Money _debtLimit;
    private readonly Money _debt;
    
    //...
        
    public static PrePaidAccount Restore(Snapshot snapshot) => new PrePaidAccount(
        snapshot.AmountAvailable,
        snapshot.DebtLimit,
        snapshot.Debt);

    private PrePaidAccount(Money amountAvailable, Money debtLimit, Money debt)
    {
        _amountAvailable = amountAvailable;
        _debtLimit = debtLimit;
        _debt = debt;
    }
        
    public void Charge(Money amount)
    {
        if (amount <= _amountAvailable)
        {
            _amountAvailable -= amount;
        }
        else if (amount <= _amountAvailable + (_debtLimit - _debt))
        {
            _debt += (amount - _amountAvailable);
            _amountAvailable -= _amountAvailable;
        }
        else
        {
            throw new DomainException();
        }
    }
    
    //...
        
    public Snapshot GetSnapshot() => new Snapshot(_amountAvailable, _debtLimit, _debt);
        
    public readonly struct Snapshot
    {
        public Money AmountAvailable { get; }
        public Money DebtLimit { get; }
        public Money Debt { get; }

        public Snapshot(Money amountAvailable, Money debtLimit, Money debt)
        {
            AmountAvailable = amountAvailable;
            DebtLimit = debtLimit;
            Debt = debt;
        }
    }
}
```

While implementing the behaviors in the domain model you should see your test scenarios starting to pass, one by one. When they are all green your model is complete.

### More features and examples

More examples of using the **BDD toolkit**  in practice can be found in the following project:  `BddToolkit.Examples.xUnit` or `BddToolkit.Examples.NUnit`.

**BDD toolkit** supports:

- Grouping scenarios by **Feature**
- Grouping scenarios by **Rule** when grouping by *Feature* is not enough
- **Tagging** of *Features* and *Scenarios*
- Adding a **Description** to every *Feature*, *Rule* and *Scenario*

## Our approach

### Why not Cucumber?

Cucumber makes a promise that the scenarios language is so easy and non-technical, that the tests can be written by business people. In reality this happens really rarely and our practice shows that usually the tests are written by someone from IT. 

Creating scenarios is a part of domain exploration and the best results are achieved when it is a *collaborative* effort of both business and IT people. Scenarios shouldn't be treated like unquestionable, waterfall requirements coming from an upstream team and thoughtlessly implemented. Because of that it is not desired to encourage business to create scenarios which will automatically become tests. 

**A tool designed for creating scenarios should focus on the best developer experience not on a business person experience**.

For developers it is far more natural to write tests with a programming language which they use on daily basis than to translate specific text files to some  calls. Automation and scenarios generation rarely works perfectly and additional effort is required to fix the generated mappings.

In addition, writing a scenario is only the beginning. On the course of time, the scenarios will probably evolve and would have to be changed. It is far easier to maintain pure code than to maintain the mappings between code and text files.

If needed, a document which is readable and understandable for business can be always generated using the tests code. It is often perfectly sufficient artifact for any further discussions with business. The difference is that it is generated only when necessary and not strictly required as a prerequisite for *any* test. 

**BDD toolkit** allows you to generate this document in a fully automated way, every time your test suite is run. This way you are sure that what you read is in a perfect sync with the real code. 

Such a generated document can use, for example, widely known Gherkin format which makes it easy to integrate **BDD toolkit** with the existing tools for scenarios visualization. Of course any other format can be used as well.

### No magic

We think that testing library should be maximally simple and transparent with a relatively flat learning curve. The tests should be easy to refactor and refine. It is all to: 

1. Reduce the amount of false positive tests
2. Make the tests **Living Documentation** which can be used to discover the business rules

It can be achieved by avoiding any "magical" solutions or conventions (like enforcing certain method names, directories structure etc). Instead we prefer to make compiler verify the test correctness. We also see that avoiding such "magic" make tests much easier to read and understand. 

### Separation of concerns 

A tool for writing BDD test scenarios shouldn't be used *instead* of a testing framework. These are two separate responsibilities, and mixing them brings absolutely no benefit. Test scenarios should work equally well regardless if we use xUnit, NUnit or MsTests. These are two separate concerns which have different architectural drivers. For example - the choice of the test framework could depend on the infrastructure you use in your CI/CD pipeline.



## Generating Docs

**BDD toolkit** makes it easy to generate documentation which is readable even for a non-technical person. We call this process documentation publishing. The publication is done directly from the code of your tests. Such a documentation can be used for communication with business experts, analysts and testers. The published scenarios can be used in any **Living Documentation** tool. 

### Configuration

To configure the documentation publishing it is enough to pass an appropriate instance of `DocPublisher` to the `BDD.Configure` method. For example: 

```c#
public void Setup() => Bdd.Configure(configuration => configuration
        .Use(DocPublishers.GherkinFiles("path")));
```
In the `DocPublisher` class you can find ready-to use implementations which come together with the **BDD toolkit**. It is also possible to write your own implementation if needed. 

**Important!** By default the documentation is not  published. You need to explicitly trigger publication. The next section - [Publishing docs](#publishing-docs) - will show you how to do it in various test frameworks.

### Publishing docs

**BDD toolkit** allows to publish documentation of already completed tests even when the whole test suite is still running. Every time the the `BDD.Publish` method is called the publication happens. Here's an example code:

```c#
public void PublishDocs() => Bdd.PublishDocs(CancellationToken.None);
```

It is recommended to publish documentation after every test *project* finishes to run. Here you can find examples on that in [xUnit](#xunit) and [NUnit](#nunit).


### Gherkin

For now **BDD toolkit** supports generating documentation files in **Gherkin** format.

#### Grouping 

**Test scenarios** can be grouped using **Features** and **Rules**. You can independently group scenarios by **Feature**  or **Rule** (or not group them at all), but if you'll use both of them,  **Rule** should be used for grouping of lower granularity (Feature can contain multiple Rules, every Rule contains multiple Scenarios) Both groupings (by Feature and by Rule) are meant to be used when your feature is more complex and contains multiple logical "rules". 

Here's a diagram showing different relationships between grouping methods.

```

FEATURE ----< SCENARIO

RULE    ----< SCENARIO

FEATURE ----< RULE ----< SCENARIO

```
#### Tagging, Descriptions and Results

In BDD toolkit every *Feature* and *Scenario* can have **Tags**  which are going to be reflected using syntax of Gherkin.

Also **Description** which can be added to every *Feature*, *Rule* or *Scenario* will be reflected using syntax of Gherkin.  

**Every test result** (*Passed*, *Failed*, *Undefined*) is added to a Gherkin scenario using comment. 

Here is an example code: 

```c#
private static readonly Feature RechargingPrePaidAccount = Bdd
    .Feature(nameof(RechargingPrePaidAccount).Humanize())
    .Description("Optional description")
    .Tags("tag1", "tag2");

private static readonly Rule DebtIsAlwaysRepaidInTheFirstPlace = Bdd
    .Rule(nameof(DebtIsAlwaysRepaidInTheFirstPlace).Humanize())
    .Feature(RechargingPrePaidAccount)
    .Description("Optional description");

[Fact]
public void DebtIsRepaidBeforeAmountAvailableIsIncreased() => Bdd.Scenario<Context>()
    .Rule(DebtIsAlwaysRepaidInTheFirstPlace)
    .Tags("tag3", "tag4")
    .Given(c => c.AmountAvailableWas(0, Currency.PLN))
    .And(c => c.DebtLimitWas(100, Currency.PLN))
    .And(c => c.DebtWas(20, Currency.PLN))
    .When(c => c.AccountIsRecharged(10, Currency.PLN))
    .Then(c => c.AmountAvailableIs(0, Currency.PLN))
    .And(c => c.DebtIs(10, Currency.PLN))
    .Test();
```

will be converted to:

```gherkin
@tag1 @tag2
Feature: Recharging pre paid account  

  Rule: Debt is always repaid in the first place

    Optional description

    # Status: Passed
    @tag3 @tag4
    Scenario: Debt is repaid before amount available is increased
      Given amount available was 0 PLN
      And debt limit was 100 PLN
      And debt was 20 PLN
      When account is recharged 10 PLN
      Then amount available is 0 PLN
      And debt is 10 PLN
```

### xUnit

Install [BDD-toolkit-dotnet.xUnit NuGet package](https://www.nuget.org/packages/BDD-toolkit-dotnet.xUnit/) in your test projects.

Add assembly attribute:

```c#
[assembly: UseBddToolkitTestFramework]
```

Add class that implements `IBddToolkitStartup`:

```c#
public class Startup : IBddToolkitStartup
{
    public void Setup(Configuration configuration) => configuration
        .Use(DocPublishers.GherkinFiles());
}
```

Sample implementation you can find in `BddToolkit.Examples.xUnit` project.

### NUnit

Add `SetUpFixture` in root namespace of your tests projects:

```c#
[SetUpFixture]
public class Startup
{
    [OneTimeSetUp]
    public void Setup() => Bdd.Configure(configuration => configuration
        .Use(DocPublishers.GherkinFiles()));

    [OneTimeTearDown]
    public void PublishDocs() => Bdd.PublishDocs(CancellationToken.None);
}
```

Sample implementation you can find in `BddToolkit.Examples.NUnit` project.

## Testing rules

Here are a few useful details of the way **BDD Toolkit** runs the tests. You can find here a list of rules which are checked every time you run a test scenario (for example the one from `ScenarioTestTests`) We recommend to read through them to see again that there is no magic in how the **BDD Toolkit** works. Here's what is checked: 

1. All `Given` actions are executed once
2. Exception in `Given` action is caught
3. `When` action is executed once
4. All `Then` actions are executed once
5. All `Then` actions are executed even if exceptions were thrown in `When` action
6. Second `Then` action is invoked even if the first assert failed
7. All exceptions from `Then` actions are reported in test result
8. Exception from `When` action is not reported for failed tests when exception check is made.
9. Test passes when exception is thrown and explicit exception check is made
10. Test fails when exception is thrown and no explicit exception check is made 

## Roadmap

1. Extending the syntax to make it possible to skip "When" in test

3. Passing Name, Description, Feature i Role using attributes

## License

The project is under [MIT license](https://opensource.org/licenses/MIT).
