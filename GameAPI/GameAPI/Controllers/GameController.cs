using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;


namespace GameAPI.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        static int pre = 0;
        static char[,] board = new char[3, 3];
        static int moves = 0;
        static String winner;
        private static String player1;
        private static String player2;
        private static String preplayer;
        private static Boolean[] visited = new Boolean[9];
        [HttpGet]
        [Logging]
        public string GetStatus()
        {
            if(moves<9)
            {
                if (checkplayer(1))
                {
                    winner = "Player1";
                    return "Player1 Won";
                }
                else if (checkplayer(2))
                {
                    winner = "Player2";
                    return "Player2 Won";
                    
                }
                else
                    return "In Progress";
            }
            else
            {
                if (checkplayer(1))
                {
                    winner = "Player1";
                    return "Player1 Won";
                }
                else if (checkplayer(2))
                {
                    winner = "Player2";
                    return "Player2 Won";
                }
                else
                {
                    winner = "Draw";
                    return "Draw";
                }
            }
        }
        public Boolean checkplayer(int playerid)
        {
            Boolean won = false;
            int column, row,counter=0;
            char playerchar;
            if (playerid == 1)
                playerchar = 'X';
            else
                playerchar = 'O';
            //row wise check
            for (column = 0; column < 3; column++)
            {
                counter = 0;
                for (row = 0; row < 3; row++)
                {
                    if (board[column, row] == playerchar)
                        counter++;
                }
                if(counter==3)
                {
                    won = true;
                    return won;
                }
            }

            //column wise check
            for (column = 0; column < 3; column++)
            {
                counter = 0;
                for (row = 0; row < 3; row++)
                {
                    if (board[row, column] == playerchar)
                        counter++;
                }
                if (counter == 3)
                {
                    won = true;
                    return won;
                }
            }
            counter = 0;
            //first diagonal check
            for(int loop=0;loop<3;loop++)
            {
                if (board[loop, loop] == playerchar)
                    counter++;
            }
            if(counter==3)
            {
                won = true;
                return won;
            }
            //second diagonal check
            counter = 0;
            for (column = 0; column < 3; column++)
            {
                for (row = 0; row < 3; row++)
                {
                    if (row + column == 2 && board[row, column] == playerchar)
                        counter++;
                }
                
            }
            if (counter == 3)
                won = true;
            return won;
        }

        // POST api/values
        [HttpPost]
        [Authorize]
        [Logging]
        public void MakeMove()
        {
           int boxid = int.Parse(HttpContext.Request.Headers["Box"].ToString());
            var apiKey = HttpContext.Request.Headers["apikey"].ToString();
            if (CheckTurn() && ValidMove(boxid))
            {
                if (winner == null)
                {
                    char movechar;
                    int row, column, counter = 1;
                    Boolean itemfound = false;
                    if (pre == 1)
                        movechar = 'O';
                    else
                        movechar = 'X';
                    
                    for (column = 0; column < 3; column++)
                    {
                        for (row = 0; row < 3; row++)
                        {
                            if (counter == boxid)
                            {
                                board[column, row] = movechar;
                                if (pre == 1)
                                    pre = 2;
                                else
                                    pre = 1;
                                itemfound = true;
                                break;
                            }
                            else
                                counter++;
                        }
                        if (itemfound == true)
                            break;
                    }
                    moves += 1;
                    using (var responseWriter = new StreamWriter(HttpContext.Response.Body))
                    {
                        GetStatus();
                        preplayer = apiKey;
                        visited[boxid - 1] = true;
                        if (winner == null)
                        {
                            responseWriter.Write("Move Successful");
                        }
                        else if (winner == "Draw")
                        {
                            responseWriter.Write(winner);
                        }
                        else
                        {
                            responseWriter.Write("Winner is=" + winner);
                        }
                    }
                }
                else
                {
                    using (var responseWriter = new StreamWriter(HttpContext.Response.Body))
                    {
                        if (winner != "Draw")
                            responseWriter.Write("Winner is=" + winner);
                        else
                            responseWriter.Write(winner);
                    }
                }
            }
        }
        public Boolean CheckTurn()
        {
            var apiKey = HttpContext.Request.Headers["apikey"].ToString();
            if (player1 == null || player1 == apiKey)
            {
                player1 = apiKey;

            }
            else if (player2 == null || player2 == apiKey)
            {
                player2 = apiKey;

            }
            if (apiKey == preplayer)
            {
                var responseWriter = new StreamWriter(HttpContext.Response.Body);
                responseWriter.Write("Not Allowed To Make a Move");
                throw new Exception("Not Allowed To Make a Move");
            }
            else if (apiKey == player1 || apiKey == player2)
                return true;
            else
            {
                var responseWriter = new StreamWriter(HttpContext.Response.Body);
                responseWriter.Write("Two Players Already Playing");
                throw new Exception("Two Players Already Playing");
            }
        }
        public Boolean ValidMove(int boxid)
        {
            if (visited[boxid - 1] == true)
            {
                var responseWriter = new StreamWriter(HttpContext.Response.Body);
                responseWriter.Write("Invalid Move");
                throw new Exception("Invalid Move");
            }
            else
                return true;
        }
    }
}
