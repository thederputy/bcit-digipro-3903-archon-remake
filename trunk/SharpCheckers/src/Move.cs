/* Move.cs : Game movement information
 * Copyright (C) 2001-2002  Paulo Pinto
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

namespace CheckersCtrl {

/// <sumary>
///  Used to represent a player game move
/// </sumary>
internal class Move {
  // Departure board house
  private int from;

  // Target board house
  private int to;


  /// <sumary>
  ///  Initializes the class
  /// </sumary>
  /// <param name="moveFrom">
  ///   Board house where the move has
  ///  initiated.
  /// </param>
  /// <param name="moveTo">
  ///   Board house where the move has
  ///  initiated.
  /// </param>
  public Move (int moveFrom, int moveTo) {
    from = moveFrom;
    to = moveTo;
  }

  /// <sumary>
  ///  from property
  /// </sumary>
  /// <value>
  ///  An integer that gives the from house used
  /// in the game move.
  /// </value>
  public int getFrom () {
    return from;
  }
  
  /// <sumary>
  ///  to property
  /// </sumary>
  /// <value>
  ///  An integer that gives the to house used
  /// in the game move.
  /// </value>
  public int getTo () {
    return to;
  }


  /// <sumary>
  ///  Returns a string representation of the class
  /// </sumary>
  public override string ToString () {
    return "(" + from + "," + to + ")";
  }
 }
}
