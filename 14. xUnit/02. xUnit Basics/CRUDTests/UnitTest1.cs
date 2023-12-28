namespace CRUDTests;

public class UnitTest1
{
    [Fact] // Every test method is decorated by this [Fact] attribute
    public void Test1()
    {
        // In unit test, in general, there 3 part of operation :
        //   ○ Arrange   declaration of variables and collecting the inputs
        //   ○ Act       calling the method
        //   ○ Assert    compare the expected value with the actual value

        // Arrange
        MyMath myMath = new();
        int input1 = 10,
            input2 = 5,
            expectedResult = 15; 

        // Act
        int actualResult = myMath.Add(input1, input2);

        // Assert
        Assert.Equal(expectedResult, actualResult); // Pay attention with the sequence of argument, look at the intellisense
    }
}