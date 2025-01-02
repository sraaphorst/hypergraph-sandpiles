// Hypergraph.cs
// By Sebastian Raaphorst, 2025.

using System.Collections.Immutable;

namespace Hypergraph.Domain;

public record Hypergraph
{
    private readonly ImmutableHashSet<Vertex> _vertices;
    private readonly ImmutableHashSet<Hyperedge> _hyperedges;
    private readonly ImmutableDictionary<Vertex, ImmutableHashSet<Hyperedge>> _hyperedgesByVertex;
    
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
    }

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
    
    // public override int GetHashCode()
    // {
    //     var hashCode = new HashCode();
    //     foreach (var vertex in _vertices.OrderBy(v => v.Id))
    //         hashCode.Add(vertex);
    //     foreach (var hyperedge in _hyperedges.OrderBy(h => h.GetHashCode()))
    //         hashCode.Add(hyperedge);
    //     return hashCode.ToHashCode();
    // }

    /// <summary>
    /// Given an existing hypergraph, enhance it with a new sink vertex that is added to each hyperedge.
    /// </summary>
    /// <param name="hypergraph">An initialized hypergraph node.</param>
    /// <param name="sink">A node designated as a sink in the sandpile problem.</param>
    /// <returns>A new Hypergraph with the sink added to the vertices and to each hyperedge.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="sink"/> is already in the <paramref name="hypergraph"/>.</exception>
    public static Hypergraph CreateWithSink(Hypergraph hypergraph, Vertex sink) 
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
