public class FizzBuzz
{
    public void CountTo(int lastNumber)
    {
        for (int actualNumber = 1; actualNumber <= lastNumber; actualNumber++)
        {
            if (actualNumber % 3 == 0 && actualNumber % 5 == 0)
            {
                System.Console.WriteLine("FizzBuzz");
            }
            else if (actualNumber % 3 == 0)
            {
                System.Console.WriteLine("Fizz");
            }
            else if (actualNumber % 5 == 0)
            {
                System.Console.WriteLine("Buzz");
            }
            else
            {
                System.Console.WriteLine(actualNumber);
            }
        }

    }
}
