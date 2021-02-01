using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyModel;

public static class Models
{
    public static GameModel gameModel;

    static Models()
    {
        gameModel = new GameModel();
    }
}
