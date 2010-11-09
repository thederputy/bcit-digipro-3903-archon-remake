/* Checkers.cs : Windows Forms class to hold the Sharp Checkers control
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
using CheckersCtrl;
using System.Diagnostics;

/// <sumary>
///  The form that holds the Checkers control.
/// </sumary>
class Checkers: Form {
  // Reference to the checkers control
  private BoardView m_view;

  // Menu options for setting the game level
  // Needed because we need to set/unset the checkmarks
  private MenuItem m_easyOpt;
  private MenuItem m_mediumOpt;
  private MenuItem m_hardOpt;

  /// <sumary> 
  ///  Creates a from with a checkers game control
  /// </sumary>
  public Checkers () {
    // Set the window title
    Text = "Sharp Checkers";

    // Set the window size
    ClientSize = new Size (300, 300);

    // Create the menu
    MainMenu menu = new MainMenu ();
    MenuItem item = new MenuItem ("&File");
    menu.MenuItems.Add (item);

    // Add the menu entries to the "File" menu    
    item.MenuItems.Add (new MenuItem ("&New Game", new EventHandler (OnNewGame)));
    item.MenuItems.Add (new MenuItem ("&Open...", new EventHandler (OnOpen)));
    item.MenuItems.Add (new MenuItem ("&Save...", new EventHandler (OnSave)));
    item.MenuItems.Add (new MenuItem ("E&xit", new EventHandler (OnExit)));

    // Create a new Menu
    item = new MenuItem ("&Options");
    menu.MenuItems.Add (item);

    // Add the menu entries to the "Options" menu
    m_easyOpt = new MenuItem ("&Easy", new EventHandler (OnEasyOpt));
    m_easyOpt.Checked = true;
    item.MenuItems.Add (m_easyOpt);
    
    m_mediumOpt = new MenuItem ("&Medium", new EventHandler (OnMediumOpt));
    item.MenuItems.Add (m_mediumOpt);

    m_hardOpt = new MenuItem ("&Hard", new EventHandler (OnHardOpt));
    item.MenuItems.Add (m_hardOpt);

    // Create a new Menu
    item = new MenuItem ("&Help");
    menu.MenuItems.Add (item);

    // Add the menu entries to the "Help" menu
    item.MenuItems.Add (new MenuItem ("&Index", new EventHandler (OnHelp)));
    item.MenuItems.Add (new MenuItem ("&About", new EventHandler (OnAbout)));

    // Attach the menu to the window
    Menu = menu;

    // Add the checkers control to the form
    m_view = new BoardView (this);
    m_view.Location = new Point (0,0);
    m_view.Size = ClientSize;
    Controls.Add (m_view);
  }


  /// <sumary> 	
  // Handler for the "New Game" option
  /// </sumary>
  private void OnNewGame (object sender, EventArgs ev) {
    // Save the current dificulty level
    int level = m_view.depth;
    m_view.newGame ();
    m_view.depth = level;
  }

  /// <sumary> 
  // Handler for the "Open" option
  /// </sumary>
  private void OnOpen (object sender, EventArgs ev) {
    Stream myStream;
    OpenFileDialog openDlg = new OpenFileDialog();

    openDlg.InitialDirectory = Directory.GetCurrentDirectory ();
    openDlg.Filter = "Checker files (*.sav)|*.sav|All files (*.*)|*.*";
    openDlg.FilterIndex = 1;
    openDlg.RestoreDirectory = true;

    if(openDlg.ShowDialog() == DialogResult.OK) {
        if((myStream = openDlg.OpenFile())!= null) {
            m_view.loadBoard (myStream);
            myStream.Close();
        }
    }        
  }

  /// <sumary> 
  /// Handler for the "Save" option
  /// </sumary>
  private void OnSave (object sender, EventArgs ev) {
    Stream myStream;
    SaveFileDialog saveDlg = new SaveFileDialog();

    saveDlg.InitialDirectory = Directory.GetCurrentDirectory ();
    saveDlg.Filter = "Checker files (*.sav)|*.sav|All files (*.*)|*.*";
    saveDlg.FilterIndex = 1;
    saveDlg.RestoreDirectory = true;

    if(saveDlg.ShowDialog() == DialogResult.OK) {
        if((myStream = saveDlg.OpenFile())!= null) {
          m_view.saveBoard (myStream); 
          myStream.Close();
        }
    }        
  }

  /// <sumary> 
  /// Handler for the "Exit" option
  /// </sumary>
  private void OnExit (object sender, EventArgs ev) {
    Close ();
  }

  /// <sumary> 	
  // Handler for the "Easy" option
  /// </sumary>
  private void OnEasyOpt (object sender, EventArgs ev) {
    m_view.depth = 1;
    m_easyOpt.Checked = true;
    m_mediumOpt.Checked = false;
    m_hardOpt.Checked = false;
  }

  /// <sumary> 	
  // Handler for the "Medium" option
  /// </sumary>
  private void OnMediumOpt (object sender, EventArgs ev) {
    m_view.depth = 3;
    m_easyOpt.Checked = false;
    m_mediumOpt.Checked = true;
    m_hardOpt.Checked = false;
  }

  /// <sumary> 	
  // Handler for the "Hard" option
  /// </sumary>
  private void OnHardOpt (object sender, EventArgs ev) {
    m_view.depth = 6;
    m_easyOpt.Checked = false;
    m_mediumOpt.Checked = false;
    m_hardOpt.Checked = true;
  }

  /// <sumary> 
  /// Handler for the "Help/Index" option
  /// </sumary>
  private void OnHelp (object sender, EventArgs ev) {
    Help.ShowHelp (this, "Checkers.chm");
  }

  /// <sumary> 
  /// Handler for the "Help/About" option
  /// </sumary>
  private void OnAbout (object sender, EventArgs ev) {
    AboutBox about = new AboutBox ();
    about.ShowDialog ();
    about = null; // Help the GC
  }

  /// <sumary> 
  /// Processes the window resizing
  /// </sumary>
  protected override void OnSizeChanged (EventArgs e) {
    base.OnSizeChanged (e);
    if (m_view != null) {
      m_view.Size = ClientSize;
      m_view.Invalidate ();
    }
  }

  /// <sumary> 
  /// Program entry point
  /// </sumary>
  public static void Main (String [] args) {
    Debug.Listeners.Add(new TextWriterTraceListener(System.Console.Out));
    Application.Run (new Checkers ());
  }
}

