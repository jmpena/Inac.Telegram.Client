using System;

namespace Inac.Telegram.Client
{
  class Program
  {
    static void Main(string[] args)
    {
      new Telegram().MainAsync(args).Wait();
    }
  }
}
