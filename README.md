# BDD toolkit for *.net*

## About

BDD toolkit is a lightweight .NET library for implementing BDD scenarios in pure C#. No regular expresions. No strict conventions. Thanks to that you can focus on the domain you test and get instantly productive.

## Why BDD toolkit?

[Here you can read more about our approach.](#our-approach)

- Fluent syntax to write tests in Given - When - Then style
- Correct exceptions handling -  all of assertions are checked even when an exception is thrown ([more info here](#testing-rules)) 
- Can be used with any test framework (xUnit, NUnit, MSTest)
- [Coming soon] Generation of readable test scenarios in text form directly from tests code (check [roadmap](#roadmap) for more info)

## Quick Start

### Installation

Install [NuGet package](https://www.nuget.org/packages/BDD-toolkit-dotnet/) in your test projects.

```powershell
Install-Package BDD-toolkit-dotnet -Version 2.0.0
```

```
dotnet add package BDD-toolkit-dotnet --version 2.0.0
```

```
<PackageReference Include="BDD-toolkit-dotnet" Version="2.0.0" />
```

### Add class for tests

First step is to create a test class for your component, feature or any other unit. In our examples it would be `PrePaidAccountTests`

Next you need to create a class for the calls to your domain code responsible for implementing  scenario steps. It could be a private nested class inside the test class created above. In our example it will be named `Context` (but it can have any other name)

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

**BDD** is a true **TDD**. When test scenarios are created with business in advance they are defined before any line of code is written and even before we have any top-level design of code!

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

Method names should represent steps from the previously created scenarios. The easiest way to define them is to first write a scenario step description in the test itself (getting the feeling of using natural language) and after use your IDE to generate the missing methods. They will be created in the class named `Context` passed to the method `Bdd.Scenario<Context>()`

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

For now there is not implementation for the defined scenarios so your tests *will* fail. However we have defined our business requirements *directly in code* which is a big win already. Now it is enough to follow the **TDD** approach i.e. implement the model and connect it to scenario by completing the methods inside the `Context` class. 

Thanks to that we get a form of **Living Documentation** Tests written this way are always up-to date with the current business requirements.

### Add domain model's types and methods

The next step is preparing the actual design of our  **domain model**. We can start with types and method headers. In our case we will need a class called `PrePaidAccount`. Its model could look like that: 

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

### Joining tests and domain model together

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

### Implementing behaviors inside domain model

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

While implementing the behaviors fo the domain model, one by one, your test scenarios will start to pass. When they are all green your model is complete.

### More examples

More examples of using the **Bdd toolkit** in practice can be found in the following project:  `BddToolkit.Examples`.

## Our approach

### Why not Cucumber?

Cucumber makes a promise that the scenarios language is so easy and non-technical that the tests can be written by business people. In reality however this happens really rarely. Usually the tests are written by someone from IT. 

Creating scenarios is a part of domain exploration and the best results are acheived when it is a *collaborative* effort of both business people and IT people. Scenarios shouldn't be treated like unquestionable, waterfall requirements coming from an upstream team and thoughtlessly implemented. Because of that it is not desired to encourge business to create scenarios which will automatically become tests. 

**A tool designed for creating scenarios should focus on the best developer experience not a business person experience**.

For developers it is far more natural to write tests using a programming language which they use on daily basis than to translate some specific text files to some method calls. Automation and scenarios generation rarely works perfectly and additional effort is required to fix the generated mappings.

In addition, writing a scenario is only the beginning. On the course of time, the scenarios will probably evolve and would have to be maintained. It is far easier to maintain pure code than to maintain the mappings between code and text files.

If needed, a document which is readable and understandable for business can be always generated using the tests code. It is often perfectly sufficient artifact for any further discussions with business. The difference is that it is generated only when necessary and not required as a source of *any* test. 

Such a generated document can use Gherkin as a format which makes it easy to integrate with many existing tools for scenarios visualisation. Of course any other format can be used as well. 


### No magic

We think that testing library should be maximally simple and transparent with a relatively flat learning curve. The tests should be easy to refactor and refine. It is all to: 

1. Reduce the amount of false positive tests
2. Make the tests **Living Documentation** which can be used to discover the business rules

It can be achieved by avoiding any "magical" solutions or conventions (like enforcing certain method names, directories structure etc). Instead we prefer to make the compiler verify the test correctness. We also see that avoiding such "magic" make the tests much easier to read and understand. 

### Separation of concerns 

A tool for writing BDD test scenarios shouldn't be used *instead* of a testing framework. These are two separate responsibilities, and mixing them brings absolutely no benefit. Test scenarios should work equally well regardless if we use xUnit, NUnit or MsTests. These are two separate concerns which have different architectural drivers. For example - the choice of the test framework could depend on the infrastructure you use in your CI/CD pipeline.

## Testing rules

All the below rules are checked by tests present in `ScenarioTestTests`: 

1. AllGivenActionsAreExecutedOnce
2. ExceptionInGivenActionsIsCaught
3. WhenActionIsExecutedOnce
4. AllThenActionsAreExecutedOnce
5. AllThenActionsAreExecutedEvenIfExceptionWasThrownInWhenAction
6. SecondThenActionIsInvokedEvenIfFirstAssertFailed
7. AllExceptionsFromThenActionsAreReportedInTestResult
8. ExceptionFromWhenActionIsNotReportedForFailedTestsInTestResultWhenExceptionCheckIsMade
9. TestPassesWhenExceptionIsThrownAndExplicitExceptionCheckIsMade
10. TestFailsWhenExceptionIsThrownAndNoExplicitExceptionCheckIsMade


## Roadmap

1. Automated Gherking docs generation out from scenarios

2. Extending the syntax to make it possible to skip "When" in test
3. Passing Name, Description, Feature i Role using attributes

## License

The project is under [MIT license](https://opensource.org/licenses/MIT).