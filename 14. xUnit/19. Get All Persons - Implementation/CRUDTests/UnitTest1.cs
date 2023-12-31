namespace CRUDTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        #region Arrange
        MyMath myMath = new();

        int input1 = 10,
            input2 = 5,
            expectedResult = 15;
        #endregion

        #region Act
        int actualResult = myMath.Add(input1, input2);
        #endregion

        #region Assert
        Assert.Equal(expectedResult, actualResult);
        #endregion
    }
}