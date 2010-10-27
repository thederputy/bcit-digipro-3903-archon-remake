package ca.bcit.cst.comp2526.assign3.solution;


import ca.bcit.cst.comp2526.assign3.solution.gui.TicTacToeFrame;
import javax.swing.JFrame;


public final class Main
{
    private Main()
    {
    }

    public static void main(final String[] argv)
    {
        final TicTacToePlayer xPlayer;
        final TicTacToePlayer oPlayer;
        final TicTacToeGame   game;
        final TicTacToeFrame  frame;
        
        game    = new TicTacToeGame();
        frame   = new TicTacToeFrame(game);
        xPlayer = new HumanTicTacToePlayer("X", frame);
        oPlayer = new ComputerTicTacToePlayer("O");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.init();
        frame.setBounds(10, 10, 300, 300);
        frame.setVisible(true);
        game.play(xPlayer, oPlayer, frame);
    }
}
