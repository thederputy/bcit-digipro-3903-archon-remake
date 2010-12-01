package ca.bcit.cst.comp2526.assign3.solution;


import java.util.regex.Matcher;
import java.util.regex.Pattern;


/**
 * A Tic Tac Toe player (X or O).
 * 
 * @author D'Arcy Smith
 * @version 1.0
 */
public abstract class TicTacToePlayer
{
    /**
     * Regex to validate the name.
     */
    private static final Pattern NAME_PATTERN;
        
    /**
     * The name of the player (either X or O).
     */
    //private final String name;
    public String name;
    
    static
    {
        NAME_PATTERN = Pattern.compile("^[XO]$");
    }
    
    /**
     * Constructs a TicTacTowPlayer with the specified name.
     * 
     * @param nm the name to give the player (either X or O)
     * @throws IllegalArgumentException if nm is null or nm is neither X or O.
     */
    protected TicTacToePlayer(final String nm)
    {
        final Matcher matcher;

        if(nm == null)
        {
            throw new IllegalArgumentException("nm cannot be null");
        }

        matcher = NAME_PATTERN.matcher(nm);

        if(!(matcher.matches()))
        {
            throw new IllegalArgumentException("\"" + nm + "\" must match " + NAME_PATTERN);
        }

        name = nm;
    }

    /**
     * Get the name.
     *
     * @return the name (either X or O).
     */
    public String getName()
    {
        return (name);
    }

    public abstract TicTacToeLocation getMove(final TicTacToeGame game);

    public void setLocation(final TicTacToeLocation loc)
    {
        throw new Error();
    }
}
