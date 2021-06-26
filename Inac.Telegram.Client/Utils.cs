using System;
using System.Collections.Generic;
using System.Text;

namespace Inac.Telegram.Client
{
  public class Utils
  {
    public static string Log(string value)
    {
      return string.Format("{0}\t{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), value);
    }
  }
}
