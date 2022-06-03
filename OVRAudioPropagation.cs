using System;
using System.Runtime.InteropServices;

namespace OVR.Audio.DotNET
{
#if _M_ARM64 || _M_AMD64 || _M_X64 || _WIN64
	using size_t = UInt64;
#else
	using size_t = UInt32;
#endif

	/** \brief A handle to a material that applies filtering to reflected and transmitted sound. 0/NULL/nullptr represent an invalid handle. */
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct ovrAudioMaterial
	{
		internal IntPtr Ptr;
	}

	/** \brief An enumeration of the scalar types supported for geometry data. */
	public enum ovrAudioScalarType {
		Int8,
		UInt8,
		Int16,
		UInt16,
		Int32,
		UInt32,
		Int64,
		UInt64,
		Float16,
		Float32,
		Float64,
    }

	/** \brief The type of mesh face that is used to define geometry.
	  *
	  * For all face types, the vertices should be provided such that they are in counter-clockwise
	  * order when the face is viewed from the front. The vertex order is used to determine the
	  * surface normal orientation.
	  */
	public enum ovrAudioFaceType
    {
		/** \brief A face type that is defined by 3 vertex indices. */
        Triangles = 0,
		/** \brief A face type that is defined by 4 vertex indices. The vertices are assumed to be coplanar. */
        Quads = 1,
        COUNT
    }

	/** \brief The properties for audio materials. All properties are frequency dependent. */
	public enum ovrAudioMaterialProperty
	{
		/** \brief The fraction of sound arriving at a surface that is absorbed by the material.
		  *
		  * This value is in the range 0 to 1, where 0 indicates a perfectly reflective material, and
		  * 1 indicates a perfectly absorptive material. Absorption is inversely related to the reverberation time,
		  * and has the strongest impact on the acoustics of an environment. The default absorption is 0.1.
		  */
		Absorption = 0,
		/** \brief The fraction of sound arriving at a surface that is transmitted through the material.
		  *
		  * This value is in the range 0 to 1, where 0 indicates a material that is acoustically opaque,
		  * and 1 indicates a material that is acoustically transparent.
		  * To preserve energy in the simulation, the following condition must hold: (1 - absorption + transmission) <= 1
		  * If this condition is not met, the transmission and absorption coefficients will be modified to
		  * enforce energy conservation. The default transmission is 0.
		  */
		Transmission = 1,
		/** \brief The fraction of sound arriving at a surface that is scattered.
		  *
		  * This property in the range 0 to 1 controls how diffuse the reflections are from a surface,
		  * where 0 indicates a perfectly specular reflection and 1 indicates a perfectly diffuse reflection.
		  * The default scattering is 0.5.
		  */
		Scattering = 2,
		COUNT
	}

	/** \brief A struct that is used to provide the vertex data for a mesh. */
	[StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct ovrAudioMeshVertices
    {
        /** \brief A pointer to a buffer of vertex data with the format described in this structure. This cannot be null. */
        public IntPtr vertices;
        /** \brief The offset in bytes of the 0th vertex within the buffer. */
        public size_t byteOffset;
        /** \brief The number of vertices that are contained in the buffer. */
        public size_t vertexCount;
        /** \brief If non-zero, the stride in bytes between consecutive vertices. */
        public size_t vertexStride;
        /** \brief The primitive type of vertex coordinates. Each vertex is defined by 3 consecutive values of this type. */
        public ovrAudioScalarType vertexType;
    }
	
	/** \brief A struct that is used to provide the index data for a mesh. */
	[StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct ovrAudioMeshIndices
    {
        /** \brief A pointer to a buffer of index data with the format described in this structure. This cannot be null. */
        public IntPtr indices;
        /** \brief The offset in bytes of the 0th index within the buffer. */
        public size_t byteOffset;
        /** \brief The total number of indices that are contained in the buffer. */
        public size_t indexCount;
        /** \brief The primitive type of the indices in the buffer. This must be an integer type. */
        public ovrAudioScalarType indexType;
    }

	/** \brief A struct that defines a grouping of mesh faces and the material that should be applied to the faces. */
	[StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct ovrAudioMeshGroup
    {
		/** \brief The offset in the index buffer of the first index in the group. */
        public size_t indexOffset;
		/** \brief The number of faces that this group uses from the index buffer.
		  *
		  * The number of bytes read from the index buffer for the group is determined by the formula: (faceCount)*(verticesPerFace)*(bytesPerIndex)
		  */
        public size_t faceCount;
		/** \brief The type of face that the group uses. This determines how many indices are needed to define a face. */
        public ovrAudioFaceType faceType;
		/** \brief A handle to the material that should be assigned to the group. If equal to 0/NULL/nullptr, a default material is used instead. */
        public ovrAudioMaterial material;
    }

	/** \brief A struct that completely defines an audio mesh. */
	[StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct ovrAudioMesh
    {
		/** \brief The vertices that the mesh uses. */
        public ovrAudioMeshVertices vertices;
		/** \brief The indices that the mesh uses. */
        public ovrAudioMeshIndices indices;
		/** \brief A pointer to an array of ovrAudioMeshGroup that define the material groups in the mesh.
		  *
		  * The size of the array must be at least groupCount. This cannot be null.
		  */
        public IntPtr groups;
		/** \brief The number of groups that are part of the mesh. */
        public size_t groupCount;
    }

	/** \brief A handle to geometry that sound interacts with. 0/NULL/nullptr represent an invalid handle. */
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct ovrAudioGeometry
	{
		internal IntPtr Ptr;
	}

	public static partial class ovrAudio
	{
		/** \brief A function pointer that reads bytes from an arbitrary source and places them into the output byte array.
		*
		* The function should return the number of bytes that were successfully read, or 0 if there was an error.
		*/
		public delegate size_t SerializerReadCallback(IntPtr userData, IntPtr bytes, size_t byteCount);

		/** \brief A function pointer that writes bytes to an arbitrary destination.
		*
		* The function should return the number of bytes that were successfully written, or 0 if there was an error.
		*/
		public delegate size_t SerializerWriteCallback(IntPtr userData, IntPtr bytes, size_t byteCount);

		/** \brief A function pointer that seeks within the data stream.
		*
		* The function should seek by the specified signed offset relative to the current stream position.
		* The function should return the actual change in stream position. Return 0 if there is an error or seeking is not supported.
		*/
		public delegate long SerializerSeekCallback(IntPtr userData, long seekOffset);
	}

	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct ovrAudioSerializer
    {
		/** \brief A function pointer that reads bytes from an arbitrary source. This pointer may be null if only writing is required. */
        public ovrAudio.SerializerReadCallback read;
		/** \brief A function pointer that writes bytes to an arbitrary destination. This pointer may be null if only reading is required. */
        public ovrAudio.SerializerWriteCallback write;
		/** \brief A function pointer that seeks within the data stream. This pointer may be null if seeking is not supported. */
        public ovrAudio.SerializerSeekCallback seek;
		/** \brief A pointer to user-defined data that will be passed in as the first argument to the serialization functions. */
    	public IntPtr userData;
    }
}