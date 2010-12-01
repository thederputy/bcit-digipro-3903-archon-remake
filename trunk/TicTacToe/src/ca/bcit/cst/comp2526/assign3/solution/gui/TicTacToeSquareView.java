package ca.bcit.cst.comp2526.assign3.solution.gui;


import ca.bcit.cst.comp2526.assign3.solution.TicTacToePiece;
import ca.bcit.cst.comp2526.assign3.solution.TicTacToePlayer;
import ca.bcit.cst.comp2526.assign3.solution.TicTacToeSquare;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.event.MouseListener;
import javax.swing.BorderFactory;
import javax.swing.JPanel;


/**
 * A view of a TicTacToeSquare.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class TicTacToeSquareView
    extends JPanel
{
    /**
     * The square being played with.
     */
    private final TicTacToeSquare square;

    /**
     * Construct a TicTacToeSquareView with the specified TicTacToeSquare.
     *
     * @param s the TicTacToeSquare to display.
     * @throws IllegalArgumentException if s is null.
     */
    public TicTacToeSquareView(final TicTacToeSquare s)
    {
        if(s == null)
        {
            throw new IllegalArgumentException("s cannot be null");
        }

        square = s;
    }

    /**
     * Initialize the view:
     * - set the boarder via BorderFactory.createEtchedBorder
     * - add the mouse listener to detect mouse releases.
     *
     * @param listener the listener to add to each TicTacToeSquareView to detect pieces being placed by the player.
     * @throws IllegalArgumentException if listener is null.
     */
    public void init(final MouseListener listener)
    {
        if(listener == null)
        {
            throw new IllegalArgumentException("listener cannot be null");
        }

        setBorder(BorderFactory.createEtchedBorder());
        addMouseListener(listener);
    }
    
    /**
     * Draw the player of the piece (if the piece is not null).
     * NOTE: This method MUST use 2 calls to drawLine to draw the X, and 1 call to drawOval to draw the O.
     * NOTE: you MUST call super.paintComponent(g) before you draw the X or O.
     *
     * @param g the graphics context.
     */
    @Override
    public void paintComponent(final Graphics g)
    {
        final TicTacToePiece piece;
        final TicTacToePlayer owner;
        final Dimension         size;

        super.paintComponent(g);
        piece = square.getPiece();

        if(piece != null)
        {
            size  = getSize();
            owner = piece.getOwner();

            if(owner.getName().equals("X"))
            {
                g.drawLine(0, 0, size.width, size.height);
                g.drawLine(0, size.height, size.width, 0);
            }
            else
            {
                g.drawOval(0, 0, size.width, size.height);
            }
        } else
        {
            size  = getSize();

            g.drawRect(0, 0, size.width, size.height);
        }
    }

    /**
     * Get the underlying TicTacToeSquare.
     *
     * @return the underlying TicTacToeSquare.
     */
    public TicTacToeSquare getSquare()
    {
        return (square);
    }
}
