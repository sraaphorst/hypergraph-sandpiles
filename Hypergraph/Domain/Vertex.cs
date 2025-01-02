// Vertex.cs
// By Sebastian Raaphorst, 2025.

namespace Hypergraph.Domain;

/// <summary>
/// Represents a unique vertex with an int identifier.
/// </summary>
public record Vertex(int Id);

// public class Vertex : IEquatable<Vertex>
// {
//     public int Id { get; }
//     public Vertex(int id) => Id = id;
//     public override string ToString() => $"Vertex({Id})";
//     public override bool Equals(object? obj) => Equals(obj as Vertex);
//     public bool Equals(Vertex? other) => other != null && other.Id == Id;
//     public override int GetHashCode() => Id.GetHashCode();
//
//     public static bool operator ==(Vertex? left, Vertex? right)
//     {
//         if (left is null)
//             return right is null;
//         return left.Equals(right);
//     }
//     public static bool operator !=(Vertex? left, Vertex? right) => !(left == right);
// }
