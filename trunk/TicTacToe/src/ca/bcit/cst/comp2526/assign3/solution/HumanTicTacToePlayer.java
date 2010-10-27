package ca.bcit.cst.comp2526.assign3.solution;

import ca.bcit.cst.comp2526.assign3.solution.gui.MouseHandler;
import ca.bcit.cst.comp2526.assign3.solution.gui.TicTacToeFrame;


public class HumanTicTacToePlayer
    extends TicTacToePlayer
{
    private TicTacToeLocation location;
    private MouseHandler handler;

    public HumanTicTacToePlayer(final String name, final TicTacToeFrame frame)
    {
        super(name);

        handler = new MouseHandler(frame);
    }

    public TicTacToeLocation getMove(final TicTacToeGame game)
    {
        final TicTacToeLocation loc;

        synchronized(this)
        {
            try
            {
                wait();
            }
            catch(final InterruptedException ex)
            {
                ex.printStackTrace();
            }
        }
        
        loc      = location;
        location = null;

        return (loc);
    }

    public void setLocation(final TicTacToeLocation loc)
    {
        location = loc;
    }
}
