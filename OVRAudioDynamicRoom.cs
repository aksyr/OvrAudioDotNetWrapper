using System;
using System.Runtime.InteropServices;

namespace OVR.Audio.DotNET
{
	// [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	// using AudioBands = Array<Single>;

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct ovrAudioVector3f
	{
		public float x,y,z;
	}

	public static partial class ovrAudio
	{
		public const int kReverbBandCount = 4;
		public const int kReverbShCoefCount = 4;

		public delegate void OVRA_RAYCAST_CALLBACK(ovrAudioVector3f origin, ovrAudioVector3f direction, out ovrAudioVector3f hit, out ovrAudioVector3f normal, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] float[] reflectionBands, IntPtr pctx);
	}
}