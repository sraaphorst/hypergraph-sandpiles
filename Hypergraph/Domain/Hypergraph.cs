// Hypergraph.cs
// By Sebastian Raaphorst, 2025.

using System.Collections.Immutable;

namespace Hypergraph.Domain;

public record Hypergraph
{
    private readonly ImmutableHashSet<Vertex> _vertices;
    private readonly ImmutableHashSet<Hyperedge> _hyperedges;
    private readonly ImmutableDictionary<Vertex, ImmutableHashSet<Hyperedge>> _hyperedgesByVertex;
    private readonly ImmutableDictionary<Vertex, ImmutableHashSet<Vertex>> _vertexNeighbours;
    private Lazy<bool> _isConnected;
    
    public IReadOnlySet<Vertex> Vertices => _vertices;
    public IReadOnlySet<Hyperedge> Hyperedges => _hyperedges;
    
    public Hypergraph(IEnumerable<Vertex> vertices, IEnumerable<Hyperedge> hyperedges)
    {
        ArgumentNullException.ThrowIfNull(vertices);
        ArgumentNullException.ThrowIfNull(hyperedges);
        _vertices = vertices.ToImmutableHashSet();
        _hyperedges = hyperedges.ToImmutableHashSet();
        
        if (_vertices.IsEmpty) throw new ArgumentException("A hypergraph must have at least one vertex.");
        if (_hyperedges.IsEmpty) throw new ArgumentException("A hypergraph must have at least one hyperedge.");
        if (!_hyperedges.All(h => h.Vertices.All(v => _vertices.Contains(v))))
            throw new ArgumentException("All hyperedges must consist of vertices in the hypergraph.");
        
        _hyperedgesByVertex = _vertices.ToImmutableDictionary(
            vertex => vertex,
            vertex => _hyperedges
                .Where(h => h.Vertices.Contains(vertex))
                .ToImmutableHashSet());

        _vertexNeighbours = _vertices.ToImmutableDictionary(
            vertex => vertex,
            vertex => _hyperedgesByVertex[vertex]
                .SelectMany(h => h.Vertices)
                .Except([vertex])
                .ToImmutableHashSet()
        );
        
        // Only calculate the connectivity of the hypergraph if requested.
        _isConnected = new Lazy<bool>(ComputeIsConnected);
    }
    
    /// <summary>
    /// Lazy computation to determine if the hypergraph is connected.
    /// </summary>
    public bool IsConnected => _isConnected.Value;

    /// <summary>
    /// Given a <see cref="Vertex"/>, return a set of the <see cref="Hyperedge"/> containing it. 
    /// </summary>
    public IReadOnlyCollection<Hyperedge> GetHyperedgesForVertex(Vertex vertex)
    {
        ArgumentNullException.ThrowIfNull(vertex);
        if (_hyperedgesByVertex.TryGetValue(vertex, out var hyperedges))
            return hyperedges;
        return ImmutableList<Hyperedge>.Empty;
    }

    public IReadOnlyCollection<Vertex> GetNeighboursForVertex(Vertex vertex)
    {
        ArgumentNullException.ThrowIfNull(vertex);
        if (_vertexNeighbours.TryGetValue(vertex, out var neighbours))
            return neighbours;
        return ImmutableList<Vertex>.Empty;
    }

    /// <summary>
    /// Return the degree of a <see cref="Vertex"/>, i.e. the number of <see cref="Hyperedge"/>s in which is
    /// appears. 
    /// </summary>
    /// <param name="vertex">A vertex in the hypergraph.</param>
    /// <returns>The number of edges in which the vertex occurs.</returns>
    /// <exception cref="ArgumentException">If the vertex is not in the hypergraph.</exception>
    public int GetVertexDegree(Vertex vertex)
    {
        ArgumentNullException.ThrowIfNull(vertex);
        if (!_vertices.Contains(vertex))
            throw new ArgumentException($"Vertex {vertex} does not exist in the hypergraph.");
        return _hyperedgesByVertex[vertex].Count;
    }

    /// <summary>
    /// Determine if the hypergraph is connected. This is called to determine if the hypergraph
    /// is connected using the lazy _isConnected variable.
    /// </summary>
    /// <returns>true is connected, and false otherwise</returns>
    private bool ComputeIsConnected()
    {
        // Hypergraphs are never empty, i.e. they always contain at least one vertex.
        var visited = new HashSet<Vertex>();
        var queue = new Queue<Vertex>();
        
        var startVertex = _vertices.First();
        visited.Add(startVertex);
        queue.Enqueue(startVertex);

        while (queue.Count > 0)
        {
            var currentVertex = queue.Dequeue();
            if (!_hyperedgesByVertex.TryGetValue(currentVertex, out var hyperedges)) continue;
            foreach (var hyperedge in hyperedges)
                foreach (var neighbour in hyperedge.Vertices)
                    if (visited.Add(neighbour))
                        queue.Enqueue(neighbour);
        }
        
        return visited.Count() == _vertices.Count;
    }

    /// <summary>
    /// Given an existing hypergraph, enhance it with a new sink vertex that is added to each hyperedge.
    /// </summary>
    /// <param name="hypergraph">An initialized hypergraph node.</param>
    /// <param name="sink">A node designated as a sink in the sandpile problem.</param>
    /// <returns>A new Hypergraph with the sink added to the vertices and to each hyperedge.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="sink"/> is already in the <paramref name="hypergraph"/>.</exception>
    public static Hypergraph AddSink(Hypergraph hypergraph, Vertex sink) 
    {
        ArgumentNullException.ThrowIfNull(hypergraph, nameof(hypergraph));
        ArgumentNullException.ThrowIfNull(sink, nameof(sink));

        var newVertices = hypergraph.Vertices.ToList();
        if (newVertices.Contains(sink))
            throw new ArgumentException("The sink is already in the collection of vertices.");
        newVertices.Add(sink);
        
        var newHyperedges = hypergraph.Hyperedges.Select(hyperedge =>
        {
            var updatedVertices = hyperedge.Vertices.ToList();
            if (updatedVertices.Contains(sink))
                throw new ArgumentException("The sink is already in a hyperedge.");
            updatedVertices.Add(sink);
            return new Hyperedge(updatedVertices);
        }).ToImmutableHashSet();
        
        return new Hypergraph(newVertices, newHyperedges);
    }
}
