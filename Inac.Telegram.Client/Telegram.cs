using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLSchema;
using TLSchema.Messages;
using TLSchema.Updates;
using TLSharp;

namespace Inac.Telegram.Client
{
  public class Telegram
  {
    public async Task MainAsync(string[] args)
    {
      Console.WriteLine(Utils.Log("Telegram Client Started..."));
      string userphone = "+17322158295";
      var client = new TelegramClient(int.Parse("5749441"),
        "114415190853d1549857ad5054e15fdc");

      await Task.Delay(500);
      await client.ConnectAsync();

      if (!client.IsUserAuthorized())
      {
        var hash = await client.SendCodeRequestAsync(userphone);
        Console.Write("Enter code sent to telegram number {0}: ", userphone);
        var code = Console.ReadLine();
        var user = await client.MakeAuthAsync(userphone, hash, code);
      }
      else
        Console.WriteLine(Utils.Log(userphone + " authenticated"));

      //get user dialogs
      var dialogs = (TLDialogs)await client.GetUserDialogsAsync();

      //find group or chat by title
      var group = dialogs.Chats
        .Where(c => c.GetType() == typeof(TLChat))
        .Cast<TLChat>()
        .FirstOrDefault(c => c.Title == "El que ta bueno no forza manito");

      while (true)
      {
        var state = await client.SendRequestAsync<TLState>(new TLRequestGetState());
        var req = new TLRequestGetDifference() { Date = state.Date, Pts = state.Pts, Qts = state.Qts };
        var diff = await client.SendRequestAsync<TLAbsDifference>(req) as TLDifference;
        if (diff != null)
        {
          //SOLO LEER LOS MENSAJES DE ESTE CANAL
          var chan = ((TLChannel)diff.Chats.FirstOrDefault());
          if (chan == null || chan.Title != "Jmtest") continue;

          foreach (var upd in diff.OtherUpdates.ToList().OfType<TLUpdateNewChannelMessage>())
          {
            var _msg = (upd.Message as TLMessage);
            if (_msg != null)
            {
              if (chan.Title == "Jmtest")
              {
                int line = 0;

                foreach (var myString in _msg.Message.Trim().Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                  var str = myString.Trim().Split(' ');
                  if (myString.Substring(0, 1) == "/")
                    break;

                  if (str[1] == "BUY" || str[1] == "SELL")
                  {
                    //BUSCAR ACCION Y PAR A OPERA FOREX
                  }
                  else if (str[0].ToUpper() == "ENTRY")
                  {

                  }
                  else if (str[0].ToUpper() == "TP1")
                  {
                  }
                  else if (str[0].ToUpper() == "TP2")
                  {
                  }
                  else if (str[0].ToUpper() == "TP3")
                  {
                  }
                  else if (str[0].ToUpper() == "TP4")
                  {
                  }
                  else if (str[0].ToUpper() == "SL")
                  {

                  }
                  line++;
                }


                  //COMO ENCONTRO UN MSG, ESCRIBIR AL GRUPO DE LOS TIGER QUE ENCONTRO ALGO
                  //send message

                  if (group != null)
                    await client.SendMessageAsync(
                      new TLInputPeerChat()
                      {
                        ChatId = group.Id
                      },
                      _msg.Message);

                  Console.WriteLine("Forex Signal:" + _msg.Message);
              }




              //                await client.SendMessageAsync(new TLInputPeerChannel() { ChannelId = chat.Id, AccessHash = chat.AccessHash.Value }, "OUR_MESSAGE");
            }
          }


          foreach (var ch in diff.Chats.ToList().OfType<TLChannel>().Where(x => !x.Left))
          {
            var ich = new TLInputChannel() { ChannelId = ch.Id, AccessHash = (long)ch.AccessHash };
            
            //var readed = new TLRequestReadHistory() { ConfirmReceived = true, c MaxId = -1 };
            //await client.SendRequestAsync<bool>(readed);
          }
        }
        await Task.Delay(500);
      }
    }
  }
}
