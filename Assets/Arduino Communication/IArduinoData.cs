using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IArduinoData
{
    public int Roll { get; }
    public int Pitch { get; }
    public int Speed { get; }
}
