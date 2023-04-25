internal class Program
{
    static void Main(string[] args)
    {
        int N = 8, numRequests = 20;
        CountdownEvent countdown = new CountdownEvent(numRequests);

        HighloadPhotoService.StartWork(N, countdown);
        for(int i  = 0; i < numRequests; i++)
        {
            HighloadPhotoService.CreateRequest(i);
        }

        countdown.Wait();
        Console.WriteLine("All requests have been processed");
    }
}

public class HighloadPhotoService
{
    static CountdownEvent countdown;
    static public void ProcessPhoto(object photo)
    {
        Random random = new Random((int)DateTime.Now.Ticks);
        Console.WriteLine($"{photo} Processing...");
        Thread.Sleep(random.Next(2000, 7000));
        Console.WriteLine($"{photo} Complete!");
        countdown.Signal();
    }
    static public void StartWork(int maxThreads, CountdownEvent Countdown)
    {
        countdown = Countdown;
        if (ThreadPool.SetMaxThreads(maxThreads, maxThreads))
            Console.WriteLine("Successful start of load sharing mechanism");
        else
            Console.WriteLine($"Something went wrong, when started the load sharing mechanism");
    }

    static public void CreateRequest(object photo)
    {
        WaitCallback photoProcessor = new WaitCallback(ProcessPhoto);
        ThreadPool.QueueUserWorkItem(photoProcessor, photo);
    }
}