package ca.bcit.cst.comp2526.assign3.solution.gui;


import ca.bcit.cst.comp2526.assign3.solution.TicTacToeGame;
import ca.bcit.cst.comp2526.assign3.solution.TicTacToePlayer;
import java.awt.event.MouseListener;
import javax.swing.JFrame;
import javax.swing.JOptionPane;


/**
 * The main frame of a TicTacToe game.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class TicTacToeFrame
    extends JFrame
{
    /**
     * The game being played.
     */
    private final TicTacToeGame game;

    /**
     * The board view.
     */
    private final TicTacToeBoardView boardView;

    /**
     * Construct a TicTacToeFrame that uses the specified game to play.
     *
     * @param g the game to play.
     * @throws IllegalArgumentException if g is null.
     */
    public TicTacToeFrame(final TicTacToeGame g)
    {
        super("Tic Tac Toe 1.0");

        if(g == null)
        {
            throw new IllegalArgumentException("g cannot be null");
        }

        game      = g;
        boardView = new TicTacToeBoardView(game.getBoard());
    }

    /**
     * Initialize the gui.
     * <p>
     * - initialze the TicTacToeBoardView
     */
    public void init()
    {
        final MouseListener handler;

        handler = new MouseHandler(this);
        boardView.init(handler);
        add(boardView);
    }

    /**
     * Get the game being played.
     *
     * @return the game being played.
     */
    public TicTacToeGame getGame()
    {
        return (game);
    }

    /**
     * Display a JOpionPane with the winner information.
     */
    public void gameOver()
    {
        final TicTacToePlayer winner;
        
        winner = game.getWinner();
        
        if(winner == null)
        {
            JOptionPane.showMessageDialog(this, "Tie Game", "Game Over", JOptionPane.INFORMATION_MESSAGE);
        }
        else
        {
            final String name;

            name = winner.getName();
            JOptionPane.showMessageDialog(this,
                                          "Player " + name + " wins!",
                                          "Game Over",
                                          JOptionPane.INFORMATION_MESSAGE);
        }
    }
}
