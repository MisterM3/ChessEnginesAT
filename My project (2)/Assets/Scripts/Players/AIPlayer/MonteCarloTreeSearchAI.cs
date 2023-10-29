using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{

    private Node parentNode;
    private ChessBoard board;
    private List<Node> childNodes;
    private int wins = 0;
    private int losses = 0;
    private int amountChecked = 0;


    public Node(Node pParentNode, ChessBoard pBoard)
    {
        childNodes = new List<Node>();
        parentNode = pParentNode;
        board = pBoard;
    }



    public void UpdateWinLoss(int result)
    {
        if (result == 1) wins++;
        if (result == -1) losses++;

        amountChecked++;
    }


    public void AddChildNode(Node childNode)
    {
        if (childNodes == null) childNodes = new List<Node>();

        childNodes.Add(childNode);
    }

    #region Getters

    public int GetWins() => wins;

    public int GetLosses() => losses;

    public int GetAmountChecked() => amountChecked;

    public List<Node> GetChildrenNodes()
    {

        if (childNodes == null) return null;

        return childNodes;

    }

    public Node GetParentNode() => parentNode;

    public ChessBoard GetBoard() => board;

    #endregion
}

[RequireComponent(typeof(SimpleEvaluation))]
public class MonteCarloTreeSearchAI : AbstractAIPlayer
{


    [SerializeField] int amountMovesSimulation = 40;

    int amount;

    SimpleEvaluation eval;

    public void Start()
    {
        eval = GetComponent<SimpleEvaluation>();
    }

    public override int EvaluateBoard(ChessBoard boardState)
    {
        throw new System.NotImplementedException();
    }

    public override void MovePiece()
    {
        Node root = new Node(null, GameBoard.Instance.chessBoardPositions);

        amount = 0;

        float timeBefore = Time.realtimeSinceStartup;
        //Amount of times simulations happen is depth
        for (int nodesTrafersed = 0; nodesTrafersed < depth; nodesTrafersed++)
        {
            Node leaf = Selection(root);
            Node expandedLeaf = Expansion(leaf);
            int result = Simulation(expandedLeaf);
            BackPropagation(expandedLeaf, result);
        }

        ChessBoard bestBoard = GetBestBoardFromChildren(root);

        float timeAfter = Time.realtimeSinceStartup;

        float timeTotal = timeAfter - timeBefore;



        if (bestBoard == null)
        {
            Debug.LogError("Error no board found");
            return; ;
        }

        Debug.Log($"Amount of moves: {amount}");


        GameBoard.Instance.ChangeBoard(bestBoard);


       // SaveData.Instance.SaveDataToFile(GameStateManager.Instance.GetTurn(), amount, timeTotal, side);

    }

    public ChessBoard GetBestBoardFromChildren(Node rootNode)
    {
        List<Node> childNodes = rootNode.GetChildrenNodes();


        if (childNodes == null || childNodes.Count == 0) return null;


        Node bestNode = null;
        float bestScore = -1;

        foreach (Node childNode in rootNode.GetChildrenNodes())
        {
            float score = (float)childNode.GetWins() / (float)childNode.GetAmountChecked();

            

            Debug.Log("Score: " + score);
            if (score > bestScore)
            {
                bestScore = score;
                bestNode = childNode;
            }
        }


        Debug.LogWarning(bestScore);
        return bestNode.GetBoard();
    }

    public override int SearchingMethod(ChessBoard boardState, int depth, ColourChessSide side)
    {
        throw new System.NotImplementedException();

    }

    #region Selection
    public Node Selection(Node selectedNode)
    {

        
        if (selectedNode == null)
        {
            Debug.LogError("SelectedNodeNull");
            return null;
        }
        

        List<Node> childNodes = selectedNode.GetChildrenNodes();

     
        if (childNodes == null || childNodes.Count == 0) return selectedNode;


        Node bestNode = null;
        float bestScore = -1;

        foreach (Node childNode in selectedNode.GetChildrenNodes())
        {
            float score = CalculateScore(childNode);

          //  Debug.Log("Node Score: " + score);
            if (score > bestScore)
            {
                bestNode = childNode;
                bestScore = score;
                
            }

            if (score == bestScore)
            {
                if (Random.Range(0, 101) < 50)
                {
                    bestNode = childNode;
                }
            }

         //   Debug.Log("Node: " + childNode.GetParentNode());
        }

        return Selection(bestNode);
    }

    //Uses expression of Szepesvári for getting value (UCT)
    public float CalculateScore(Node selectedNode)
    {
        float wi = selectedNode.GetWins();
        float ni = selectedNode.GetAmountChecked();

        if (ni == 0) return int.MaxValue;

        //float c = Mathf.Sqrt(2);

        float c = 2;

        float Ni = selectedNode.GetParentNode().GetAmountChecked();


        float value = (wi / ni) + c * Mathf.Sqrt((Mathf.Log(Ni) / ni));


        Debug.Log(wi);

        Debug.Log(ni);

        Debug.Log(wi / ni);


        return value;
    }

    #endregion

    public Node Expansion(Node selectedNode)
    {
        if (selectedNode == null)
        {
            Debug.LogError("SelectedNodeNull");
            return null;
        }


        List<Node> allChildNodes = new List<Node>();


        foreach (Pieces piece in selectedNode.GetBoard().GetChessBoard())
        {
            if (piece == null) continue;

            if (piece.colourPiece != side) continue;

            List<Vector2Int> moves = piece.GetLegalMoves();

            //Castling+Promotions
            List<ChessBoard> specialBoards = new List<ChessBoard>();

            if (moves == null || moves.Count == 0) continue;
            foreach (Vector2Int move in moves)
            {

                ChessBoard newBoard = GetNewBoard(piece, selectedNode.GetBoard(), move);


                //Promotions
                if (piece is Pawns)
                {
                    if ((move.y == 7 && piece.colourPiece == ColourChessSide.White) || (move.y == 0 && piece.colourPiece == ColourChessSide.Black))
                    {
                        ChessBoard QueenPromotion = newBoard.CopyBoard();
                        Queen queen = new Queen();
                        queen.colourPiece = piece.colourPiece;
                        QueenPromotion.SetPieceAtPosition(move, queen);

                        ChessBoard BishopPromotion = newBoard.CopyBoard();
                        Bishop bishop = new Bishop();
                        bishop.colourPiece = piece.colourPiece;
                        BishopPromotion.SetPieceAtPosition(move, bishop);

                        ChessBoard RookPromotion = newBoard.CopyBoard();
                        Rook rook = new Rook();
                        rook.colourPiece = piece.colourPiece;
                        RookPromotion.SetPieceAtPosition(move, bishop);

                        ChessBoard KnightPromotion = newBoard.CopyBoard();
                        Knight knight = new Knight();
                        knight.colourPiece = piece.colourPiece;
                        KnightPromotion.SetPieceAtPosition(move, bishop);

                        specialBoards.Add(QueenPromotion);
                        specialBoards.Add(BishopPromotion);
                        specialBoards.Add(RookPromotion);
                        specialBoards.Add(KnightPromotion);

                        continue;
                    }
                }

                Node childNode = new Node(selectedNode, newBoard);
                selectedNode.AddChildNode(childNode);
                allChildNodes.Add(childNode);
                amount++;

            }


            List<ChessBoard> castling = selectedNode.GetBoard().GetCastlingMoves(side);

            if (castling != null && castling.Count != 0)
            {
                foreach (ChessBoard board in castling)
                {
                    specialBoards.Add(board);
                }
            }

            if (specialBoards == null || specialBoards.Count == 0) continue;

            foreach (ChessBoard board in specialBoards)
            {
                ChessBoard newBoard = board.CopyBoard();

                Node childNode = new Node(selectedNode, newBoard);
                selectedNode.AddChildNode(childNode);
                allChildNodes.Add(childNode);
                amount++;
            }
        }

        if (allChildNodes == null || allChildNodes.Count == 0) return null;

        return allChildNodes[Random.Range(0, allChildNodes.Count)];
    }


    public int Simulation(Node node)
    {
        if (node == null)
        {
            Debug.LogError("SelectedBoardNull");
            return 0;
        }


        ChessBoard simulationBoard = node.GetBoard();

        ColourChessSide sideRandomMove = side;

        for (int i = 0; i <= amountMovesSimulation; i++)
        {
            ChessBoard simBoard = SelectRandomMove(simulationBoard, sideRandomMove);

            if (simBoard == null) break; 

            

            if (sideRandomMove == ColourChessSide.White) sideRandomMove = ColourChessSide.Black;
            else if (sideRandomMove == ColourChessSide.Black) sideRandomMove = ColourChessSide.White;
        }

        float score = eval.Evaluate(simulationBoard);

        if (side == ColourChessSide.Black) score *= -1;

        if (score > 1) return 1;
        else if (score < -1) return -1;
        else return 0;

    }
    /// <summary>
    /// Gets random move, and returns the chessboard with the new move
    /// </summary>
    /// <returns></returns>
    public ChessBoard SelectRandomMove(ChessBoard selectedBoard, ColourChessSide side)
    {

        if (selectedBoard == null)
        {
            Debug.LogError("SelectedBoardNull");
            return null;
        }

        List<ChessBoard> allChildBoards = new List<ChessBoard>();

        foreach (Pieces piece in selectedBoard.GetChessBoard())
        {
            if (piece == null) continue;

            if (piece.colourPiece != side) continue;

            List<Vector2Int> moves = piece.GetLegalMoves();

            //Castling+Promotions
            List<ChessBoard> specialBoards = new List<ChessBoard>();

            if (moves == null || moves.Count == 0) continue;
            foreach (Vector2Int move in moves)
            {

                ChessBoard newBoard = GetNewBoard(piece, selectedBoard, move);


                //Promotions
                if (piece is Pawns)
                {
                    if ((move.y == 7 && piece.colourPiece == ColourChessSide.White) || (move.y == 0 && piece.colourPiece == ColourChessSide.Black))
                    {
                        ChessBoard QueenPromotion = newBoard.CopyBoard();
                        Queen queen = new Queen();
                        queen.colourPiece = piece.colourPiece;
                        QueenPromotion.SetPieceAtPosition(move, queen);

                        ChessBoard BishopPromotion = newBoard.CopyBoard();
                        Bishop bishop = new Bishop();
                        bishop.colourPiece = piece.colourPiece;
                        BishopPromotion.SetPieceAtPosition(move, bishop);

                        ChessBoard RookPromotion = newBoard.CopyBoard();
                        Rook rook = new Rook();
                        rook.colourPiece = piece.colourPiece;
                        RookPromotion.SetPieceAtPosition(move, bishop);

                        ChessBoard KnightPromotion = newBoard.CopyBoard();
                        Knight knight = new Knight();
                        knight.colourPiece = piece.colourPiece;
                        KnightPromotion.SetPieceAtPosition(move, bishop);

                        specialBoards.Add(QueenPromotion);
                        specialBoards.Add(BishopPromotion);
                        specialBoards.Add(RookPromotion);
                        specialBoards.Add(KnightPromotion);

                        continue;
                    }
                }

                allChildBoards.Add(newBoard);
                
            }


            List<ChessBoard> castling = selectedBoard.GetCastlingMoves(side);

            if (castling != null && castling.Count != 0)
            {
                foreach (ChessBoard board in castling)
                {
                    specialBoards.Add(board);
                }
            }

            if (specialBoards == null || specialBoards.Count == 0) continue;

            foreach (ChessBoard board in specialBoards)
            {
                ChessBoard newBoard = board.CopyBoard();

                allChildBoards.Add(newBoard);
            }
        }


        Debug.Log(allChildBoards.Count);

        if (allChildBoards == null || allChildBoards.Count == 0) return null;

        return allChildBoards[Random.Range(0, allChildBoards.Count)];
    }




    //Update scores of all the nodes
    public void BackPropagation(Node node, int result)
    {
        if (node == null) return;


        Debug.LogWarning("SCore end:" + result);

        node.UpdateWinLoss(result);

        //No parent node, thus root node has been reached
        if (node.GetParentNode() == null) return;

        BackPropagation(node.GetParentNode(), result);
    }
    private ChessBoard GetNewBoard(Pieces piece, ChessBoard oldBoard, Vector2Int move)
    {
        Vector2Int oldGridPosition = piece.gridPosition;


        ChessBoard newBoard = oldBoard.CopyBoard();

        Pieces copiedPiece = newBoard.GetPieceFromPosition(oldGridPosition);


        //Sets moved to false
        if (copiedPiece is ISpecialFirstMove)
        {
            ISpecialFirstMove movePiece = (ISpecialFirstMove)copiedPiece;
            movePiece.FirstMove();

            copiedPiece = (Pieces)movePiece;
        }
        newBoard.SetPieceAtPosition(move, copiedPiece);
        newBoard.SetPieceAtPosition(oldGridPosition, null);


        return newBoard;
    }
}
