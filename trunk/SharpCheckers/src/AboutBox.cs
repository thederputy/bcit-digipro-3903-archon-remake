/* AboutBox.cs : Shows an about box for the game
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


// Bring the needed packages into the global scope
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Resources;

public class AboutBox: Form {
  public AboutBox () {
    // Set the window title
    Text = "About Sharp Checkers";

    // Set the window size
    ClientSize = new Size (400, 130);

    // Don't allow resize of the box
    FormBorderStyle = FormBorderStyle.FixedDialog;
    MinimizeBox = false;
    MaximizeBox = false;

    // Create the picture box for the image
    PictureBox picture = new PictureBox ();
    ResourceManager resources = new ResourceManager(typeof(AboutBox));
    picture.Image = ((System.Drawing.Bitmap)(resources.GetObject("AboutBox.Logo")));
    picture.Location = new Point(8, 8);
    picture.Size = new Size(152, 144);
    Controls.Add (picture);

    // Create the label for the copyright
    AddLabel (new Point(170, 8), new Size(128, 16), "(c) 2001 - Paulo Pinto");

    // Create the labels for the license message
    AddLabel (new Point(170, 72), new Size(120, 23), "This game is under the");
    AddLinkLabel (new Point(290, 72), new Size(32, 16), "GPL");
    AddLabel (new Point(321, 72), new Size(40, 23), "license");

    // Create the label for the version
    AddLabel (new Point(170, 40), new Size(72, 16), "Version 0.1.1");

    // Create an 'OK' button
    Button btn = new Button ();
    btn.Text = "&OK";
    btn.Location = new Point (250, 100);
    btn.Click += new EventHandler (OnClick);

    // This button is the default one
    AcceptButton = btn;
  
    // The box should appear in the middle of the screen
    StartPosition = FormStartPosition.CenterScreen;

    // Add the button to the form
    Controls.Add (btn);
  }

  /// <sumary> 
  /// Helper method to add the labels to the form in a
  /// generic way.
  /// </sumary>
  private void AddLabel (Point location, Size area, string str) {
    Label label = new Label ();

    // Set the label message
    label.Text = str;

    // Set the location inside the form
    label.Location = location;

    // Set the label dimension
    label.Size = area;

    // Add the Label to the form
    Controls.Add (label);
  }

  /// <sumary> 
  /// Helper method to add the link labels to the form in a
  /// generic way.
  /// </sumary>
  private void AddLinkLabel (Point location, Size area, string str) {
    LinkLabel label = new LinkLabel ();

    // Set the label message
    label.Text = str;

    // Set the location inside the form
    label.Location = location;

    // Set the label dimension
    label.Size = area;

    // Something must be done when the control is clicked
    label.Click += new EventHandler (OnLinkClicked);

    // Add the Label to the form
    Controls.Add (label);
  }

  /// <sumary> 
  /// Handler for the "Click" event in the 'OK' button
  /// </sumary>
  private void OnClick (object sender, EventArgs ev) {
    Close ();
  }

  /// <sumary> 
  /// Handler for the "Click" event in the 'GPL' link
  /// </sumary>
  private void OnLinkClicked(object sender, EventArgs ev) {
    Process.Start("IExplore.exe", "http://www.gnu.org/copyleft/gpl.html");
  }

}
