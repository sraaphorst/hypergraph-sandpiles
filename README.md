# Hypergraph Sandpiles

This is an experimental project in my process of learning C# to implement
a project that experiments with [sandpiles](https://en.wikipedia.org/wiki/Abelian_sandpile_model)
over various families of [hypergraphs](https://en.wikipedia.org/wiki/Hypergraph) that can be represented by grids.

Of particular interest, I would like to explore:
- Sandpiles over oroidal hypergraphs with various kinds of hyperedges.
  - A hyperedge of size 5 centered on each vertex (the [Von Neumann neighbourhood](https://en.wikipedia.org/wiki/Von_Neumann_neighborhood) and a sink vertex).
  - A hyperedge of size 9 centered on each vertex (the [Moore neighbourhood](https://en.wikipedia.org/wiki/Moore_neighborhood) and a sink vertex).
- Sandpiles over chess boards: given a chess piece, P (e.g. knight), the neighbourhood would comprise all the possible
  positions to which P could move. (This would lose any abelian property unless moves are bidirectional). This could be done over a torus or on a
  grid, and the sink in a torus could be included in each hyperedge, or, if on a grid, in the natural way.
- Sandpiles over combinatorial structures.
  - [Block designs](https://en.wikipedia.org/wiki/Block_design#Generalization:_t-designs) possibly including
    [Steiner systems](https://en.wikipedia.org/wiki/Steiner_system), if we can derive a sane toppling rule that does not
    amount to firing to every adjacent vertex.
  - [Latin squares](https://en.wikipedia.org/wiki/Latin_square), where each vertex is in three hyperedges, namely
    the row to which it belongs, the column to which it belongs, and the squares with the same symbol.
  - [Sudoku boards](https://en.wikipedia.org/wiki/Sudoku), where the neighbourhood of each vertex would be as in the
    latin square case, but also include the 3x3 subgrid a vertex appears in. (Care to not fire twice to the same vertex
    would be a consideration).

I will begin by implementing first the basic framework of the hypergraphs, some of the hypergraphs above, and the sandpile
logic, and then proceed to the logic mapping hypergraphs to grids and the display logic using Avalonia.UI.