package ca.bcit.cst.comp2526.assign3.solution;


/**
 * Thrown when a location is created that is outside the allowable range.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class InvalidLocationException
    extends Exception
{
    /**
     * Constructs an InvalidLocationException with the specified values.
     *
     * @param name the name of the variables (eg. r, row, c, or col.
     * @param min the smallest allowable value.
     * @param max the largest allowable value.
     * @param value the value passed in.
     */
    public InvalidLocationException(final String name,
                                    final int    min,
                                    final int    max,
                                    final int    value)
    {
        super(name + " must be >= " + min + " and <= " + max + ", was: " + value);
    }
}
