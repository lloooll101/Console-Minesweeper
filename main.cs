using System;

class Program {
  public static void Main (string[] args) {
    //Prompt the user for game specifications
    Console.WriteLine("Width of Field:");
    int inputX = int.TryParse(Console.ReadLine(), out inputX) ? inputX : 10;
    
    Console.WriteLine("Height of Field:");
    int inputY = int.TryParse(Console.ReadLine(), out inputY) ? inputY : 10;
    
    Console.WriteLine("Number of Mines:");
    int inputMines = int.TryParse(Console.ReadLine(), out inputMines) ? inputMines : 10;

    //Start the minesweeper game
    Minesweeper minesweeper = new Minesweeper(inputX, inputY, inputMines);

    //Main game loop
    while(true)
    {
      Console.Clear();
      Console.WriteLine(minesweeper.Display());
      //Prompt user for input
      Console.WriteLine("Do you want to reveal or flag?");
      string input = Console.ReadLine().ToLower();
      
      if(input == "reveal")
      {
        Console.WriteLine("Reveal tile:");
        inputX = Int32.Parse(Console.ReadLine());
        inputY = Int32.Parse(Console.ReadLine());
        minesweeper.Reveal(inputX, inputY);
      }
      else if(input == "reveal area")
      {
        Console.WriteLine("Reveal area:");
        inputX = Int32.Parse(Console.ReadLine());
        inputY = Int32.Parse(Console.ReadLine());
        minesweeper.RevealArea(inputX, inputY);
      }
      else if(input == "flag")
      {
        Console.WriteLine("Flag tile:");
        inputX = Int32.Parse(Console.ReadLine());
        inputY = Int32.Parse(Console.ReadLine());
        minesweeper.Flag(inputX, inputY);
      }
      
    }
  }
}

class Minesweeper {
  public int[,] mineGrid;
  public bool[,] revealed;
  public bool[,] flagged;
  Random rnd = new Random();

  public Minesweeper(int x, int y, int mines)
  {
    //Generate a new game
    newGame(x, y, mines);
  }

  public void Reveal(int x, int y)
  {
    //Just put everything in a try catch to ignore when cofdinates are out of bounds
    try
    {
      //If tile is flagged, do nothing
      if(flagged[x, y]){return;}
      //Set the tile to revealed
      revealed[x, y] = true;

      //Scans each tile and reveals nearby tiles if current tile has 0 surrounding mines
      //There is probably a better way of doing this, but it works well enough
      while(true)
      {
        bool done = true;
        for(int i = 0; i < mineGrid.GetLength(0); i++)
        {
          for(int j = 0; j < mineGrid.GetLength(1); j++)
          {
            if(mineGrid[i, j] == 0 && revealed[i, j] == true)
            {
              /*Expanded segment:
              try
              {
                if(revealed[i - 1, j - 1] == false)
                {
                  revealed[i - 1, j - 1] = true;
                  done = false;
                }
              }catch{}
              */
              //Try catch because it can go out of bounds
              try{if(revealed[i - 1, j - 1] == false){revealed[i - 1, j - 1] = true;done = false;}}catch{}
              try{if(revealed[i - 1, j + 0] == false){revealed[i - 1, j + 0] = true;done = false;}}catch{}
              try{if(revealed[i - 1, j + 1] == false){revealed[i - 1, j + 1] = true;done = false;}}catch{}
              try{if(revealed[i + 0, j - 1] == false){revealed[i + 0, j - 1] = true;done = false;}}catch{}
              try{if(revealed[i + 0, j + 1] == false){revealed[i + 0, j + 1] = true;done = false;}}catch{}
              try{if(revealed[i + 1, j - 1] == false){revealed[i + 1, j - 1] = true;done = false;}}catch{}
              try{if(revealed[i + 1, j + 0] == false){revealed[i + 1, j + 0] = true;done = false;}}catch{}
              try{if(revealed[i + 1, j + 1] == false){revealed[i + 1, j + 1] = true;done = false;}}catch{}
            }
          }
        }
        if(done){break;}
      }

      //Check each revealed tile for mines
      for(int i = 0; i < mineGrid.GetLength(0); i++)
      {
        for(int j = 0; j < mineGrid.GetLength(1); j++)
        {
          if(revealed[i, j] && mineGrid[x, y] == 9)
          {
            //TODO: quit game
          }
        }
      }
    }
    catch{}
  }

  //Because if you thought the above was bad enough, I now make it run 9 times
  //Why loop when you can just write it out
  public void RevealArea(int x, int y)
  {
    Reveal(x-1, y-1);
    Reveal(x-1, y+0);
    Reveal(x-1, y+1);
    Reveal(x+0, y-1);
    Reveal(x+0, y+0);
    Reveal(x+0, y+1);
    Reveal(x+1, y-1);
    Reveal(x+1, y+0);
    Reveal(x+1, y+1);
  }

  //Change the flagged variable
  public void Flag(int x, int y)
  {
    flagged[x, y] = !flagged[x, y];
  }

  //Pain
  public string Display()
  {
    //Add seperation
    string output = "\n\n\n\n      ";
    
    //Add column index
    for(int x = 0; x < mineGrid.GetLength(0); x++)
    {
      output += x.ToString();
      //Add spacing based on the length of the index number
      for(int i = 0; i < 4 - x.ToString().Length; i++)
      {
        output += " ";
      }
    }
    
    //Spacer
    output  += "\n";
    
    for(int y = 0; y < mineGrid.GetLength(1); y++)
    {
      //Add row index
      output += "\n\n" + y.ToString();
      //Add spacing based on the length of the index number
      for(int i = 0; i < 6 - y.ToString().Length; i++)
      {
        output += " ";
      }

      //Add each tile
      for(int x = 0; x < mineGrid.GetLength(0); x++)
      {
        if(revealed[x, y])
        {
          if(mineGrid[x, y] == 0)
          {
            //Blank tile
            output += "    ";
          }
          else if(mineGrid[x, y] == 9)
          {
            //Mine
            output += "X   ";
          }
          else
          {
            //Number
            output += mineGrid[x, y];
            output += "   ";
          }
        }
        else if(flagged[x, y])
        {
          //Flag
          output += "⚑   ";
        }
        else
        {
          //Unrevealed
          output += "█   ";
        }
      }
    }
    
    return output;
  }

  public void newGame(int x, int y, int mines)
  {

    //Confirm that the field has a somewhat playable amonut of mines
    if(mines > x * y * 0.5)
    {
      mines = (int)(x * y * 0.5);
    }
    
    //Replace the grid with a new array
    mineGrid = new int[x, y];
    revealed = new bool[x, y];
    flagged = new bool[x, y];
    
    //Set all tiles to 0, not revealed, and not flagged
    for(int i = 0; i < x; i++)
    {
      for(int j = 0; j < y; j++)
      {
        mineGrid[i, j] = 0;
        revealed [i, j] = false;
        flagged[i, j] = false;
      }
    }

    //Add mines and update surrounding tiles
    for(int i = 0; i < mines; i++)
    {
      int randomX = rnd.Next(0, x);
      int randomY = rnd.Next(0, y);

      //Set the random tile to 9, unless it is already a mine. Numbers greater than 9 represent a mine
      if(mineGrid[randomX, randomY] < 9)
      {
        mineGrid[randomX, randomY] = 9;
        //Increases the surrounding tiles by 1
        for(int j = randomX - 1; j < randomX + 2; j++)
        {
          for(int k = randomY - 1; k < randomY + 2; k++)
          {
            try{mineGrid[j, k]++;}catch{}
          }
        }
      }
      else
      {
        //If it is already a mine, go back and try again
        i--;
      }
    }
    
    //Limit max number to 9
    for(int i = 0; i < mineGrid.GetLength(0); i++)
    {
      for(int j = 0; j < mineGrid.GetLength(1); j++)
      {
        if(mineGrid[i, j] > 9)
        {
          mineGrid[i, j] = 9;
        }
      }
    }
  }
}