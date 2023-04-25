internal class Program
{
    static void Main(string[] args)
    {
        int N = 4, numRequests = 20;
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
    static int maxThreads;
    static CountdownEvent countdown;
    static public void ProcessPhoto(object photo)
    {
        Random random = new Random((int)DateTime.Now.Ticks);
        Console.WriteLine("Processing...");
        Thread.Sleep(random.Next(200, 1700));
        Console.WriteLine("Complete!");
        countdown.Signal();
    }
    static public void StartWork(int N, CountdownEvent Countdown)
    {
        maxThreads = N;
        countdown = Countdown;
        ThreadPool.SetMaxThreads(maxThreads, maxThreads);
    }

    static public void CreateRequest(object photo)
    {
        WaitCallback photoProcessor = new WaitCallback(ProcessPhoto);
        ThreadPool.QueueUserWorkItem(photoProcessor, photo);
    }
}