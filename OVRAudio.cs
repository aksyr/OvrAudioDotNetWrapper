using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OVR.Audio.DotNET
{
    using ovrResult = Int32;

#if _M_ARM64 || _M_AMD64 || _M_X64 || _WIN64
	using size_t = UInt64;
#else
	using size_t = UInt32;
#endif

    public enum ovrAudioError : int
    {
        AudioUnknown                                = 2000, ///< An unknown error has occurred.
        AudioInvalidParam                           = 2001, ///< An invalid parameter, e.g. NULL pointer or out of range variable, was passed
        AudioBadSampleRate                          = 2002, ///< An unsupported sample rate was declared
        AudioMissingDLL                             = 2003, ///< The DLL or shared library could not be found
        AudioBadAlignment                           = 2004, ///< Buffers did not meet 16b alignment requirements
        AudioUninitialized                          = 2005, ///< audio function called before initialization
        AudioHRTFInitFailure                        = 2006, ///< HRTF provider initialization failed
        AudioBadVersion                             = 2007, ///< Mismatched versions between header and libs
        AudioSymbolNotFound                         = 2008, ///< Couldn't find a symbol in the DLL
        SharedReverbDisabled                        = 2009, ///< Late reverberation is disabled
        AudioNoAvailableAmbisonicInstance           = 2017,
        AudioMemoryAllocFailure                     = 2018,
        AudioUnsupportedFeature                     = 2019, ///< Unsupported feature
        AudioInternalEnd                            = 2099, ///< Internal errors used by Audio SDK defined down towards public errors
                                                                        ///< NOTE: Since we do not define a beginning range for Internal codes, make sure
                                                                        ///< not to hard-code range checks (since that can vary based on build)
    }

    /// Audio source flags
    ///
    /// \see ovrAudio_SetAudioSourceFlags
    [Flags]
    public enum ovrAudioSourceFlags : int
    {
        None                          = 0x0000,

        WideBand_HINT                 = 0x0010, ///< Wide band signal (music, voice, noise, etc.)
        NarrowBand_HINT               = 0x0020, ///< Narrow band signal (pure waveforms, e.g sine)
        BassCompensation_DEPRECATED   = 0x0040, ///< Compensate for drop in bass from HRTF (deprecated)
        DirectTimeOfArrival           = 0x0080, ///< Time of arrival delay for the direct signal

        ReflectionsDisabled           = 0x0100, ///< Disable reflections and reverb for a single AudioSource

        #if OVR_INTERNAL_CODE
            Stereo					   = 0x0200, ///< Stereo AudioSource
        #endif

        DisableResampling_RESERVED    = 0x8000, ///< Disable resampling IR to output rate, INTERNAL USE ONLY
    }

    /// Audio source attenuation mode
    ///
    /// \see ovrAudio_SetAudioSourceAttenuationMode
    public enum ovrAudioSourceAttenuationMode : int
    {
        None          = 0,      ///< Sound is not attenuated, e.g. middleware handles attenuation
        Fixed         = 1,      ///< Sound has fixed attenuation (passed to ovrAudio_SetAudioSourceAttenuationMode)
        InverseSquare = 2,      ///< Sound uses internally calculated attenuation based on inverse square

        COUNT
    }

    /// Global boolean flags
    ///
    /// \see ovrAudio_Enable
    public enum ovrAudioEnable : int
    {
        None                     = 0,   ///< None
        SimpleRoomModeling       = 2,   ///< Enable/disable simple room modeling globally, default: disabled
        LateReverberation        = 3,   ///< Late reverbervation, requires simple room modeling enabled
        RandomizeReverb          = 4,   ///< Randomize reverbs to diminish artifacts.  Default: enabled.

        COUNT
    }

    /// Explicit override to select reflection and reverb system
    ///
    /// \see ovrAudio_SetReflectionModel
    public enum ovrAudioReflectionModel : int
    {
        StaticShoeBox       = 0,    ///< Room controlled by ovrAudioBoxRoomParameters
        DynamicRoomModeling = 1,    ///< Room automatically calculated by raycasting using OVRA_RAYCAST_CALLBACK
        PropagationSystem   = 2,    ///< Sound propgated using game geometry
        Automatic           = 3,    ///< Automatically select highest quality (if geometry is set the propagation system rwise if the callback is set dynamic room modeling is enabled, otherwise fallback to the static shoe box)


        COUNT
    }

    /// Internal use only
    ///
    /// Internal use only
    public enum ovrAudioHRTFInterpolationMethod : int
    {
        Nearest,
        SimpleTimeDomain,
        MinPhaseTimeDomain,
        PhaseTruncation,
        PhaseLerp,

        COUNT
    }

    /// Status mask returned by spatializer APIs
    ///
    /// Mask returned from spatialization APIs consists of combination of these.
    /// \see ovrAudio_SpatializeMonoSourceLR
    /// \see ovrAudio_SpatializeMonoSourceInterleaved
    public enum ovrAudioSpatializationStatus : int
    {
        None      = 0x00,  ///< Nothing to report
        Finished  = 0x01,  ///< Buffer is empty and sound processing is finished
        Working   = 0x02,  ///< Data still remains in buffer (e.g. reverberation tail)
    }

    /// Performance counter enumerants
    ///
    /// \see ovrAudio_GetPerformanceCounter
    /// \see ovrAudio_SetPerformanceCounter
    public enum ovrAudioPerformanceCounter : int
    {
        Spatialization            = 0,  ///< Retrieve profiling information for spatialization
        SharedReverb              = 1,  ///< Retrieve profiling information for shared reverb

        COUNT
    }

    /// Ambisonic formats
    public enum ovrAudioAmbisonicFormat : int
    {
        FuMa, ///< standard B-Format, channel order = WXYZ (W channel is -3dB)
        AmbiX ///< ACN/SN3D standard, channel order = WYZX
    }

    /// Ambisonic rendering modes
    ///
    /// NOTE: Support for rendering ambisonics via virtual speaker layouts has been
    /// discontinued in favor of improved decoding with spherical harmonics, which
    /// uses no virtual speakers at all and provides better externalization.
    ///
    public enum ovrAudioAmbisonicRenderMode : int
    {
        SphericalHarmonics  = -1, ///< (default) Uses a spherical harmonic representation of HRTF
        Mono                = -2  ///< Plays the W (omni) channel through left and right with no spatialization
    }

    public struct ovrPosef {}

    public struct ovrPoseStatef {}

    public struct ovrAudioSource {}

    [StructLayout(LayoutKind.Sequential)]
    public struct ovrAudioContext
    {
        internal IntPtr Ptr;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ovrAudioAmbisonicStream
    {
        internal IntPtr Ptr;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ovrAudioSpectrumAnalizer
    {
        internal IntPtr Ptr;
    }

    [StructLayout(LayoutKind.Sequential)][Serializable]
    public struct ovrAudioContextConfiguration
    {
        internal uint acc_Size;             ///< set to size of the struct
        public uint acc_MaxNumSources;    ///< maximum number of audio sources to support
        public uint acc_SampleRate;       ///< sample rate (16000 to 48000, but 44100 and 48000 are recommended for best quality)
        public uint acc_BufferLength;     ///< number of samples in mono input buffers passed to spatializer

        public override string ToString()
        {
            return $"ovrAudioContextConfiguration(acc_Size={acc_Size}, acc_MaxNumSources={acc_MaxNumSources}, acc_SampleRate={acc_SampleRate}, acc_BufferLength={acc_BufferLength})";
        }
    }

    /// Box room parameters used by ovrAudio_SetSimpleBoxRoomParameters
    ///
    /// \see ovrAudio_SetSimpleBoxRoomParameters
    [StructLayout(LayoutKind.Sequential)][Serializable]
    public struct ovrAudioBoxRoomParameters
    {
        internal uint brp_Size;                              ///< Size of struct
        public float    brp_ReflectLeft, brp_ReflectRight;     ///< Reflection values (0 - 0.97)
        public float    brp_ReflectUp, brp_ReflectDown;        ///< Reflection values (0 - 0.97)
        public float    brp_ReflectBehind, brp_ReflectFront;   ///< Reflection values (0 - 0.97)
        public float    brp_Width, brp_Height, brp_Depth;      ///< Size of box in meters

        public override string ToString()
        {
            return $"ovrAudioBoxRoomParameters(brp_Size={brp_Size}, brp_ReflectLeft={brp_ReflectLeft}, brp_ReflectRight={brp_ReflectRight}, brp_ReflectUp={brp_ReflectUp}, brp_ReflectDown={brp_ReflectDown}, brp_ReflectBehind={brp_ReflectBehind}, brp_ReflectFront={brp_ReflectFront}, brp_Width={brp_Width}, brp_Height={brp_Height}, brp_Depth={brp_Depth})";
        }
    }

    /// Box room parameters used by ovrAudio_SetSimpleBoxRoomParameters
    ///
    /// \see ovrAudio_SetAdvancedBoxRoomParameters
    [StructLayout(LayoutKind.Sequential)][Serializable]
    struct ovrAudioAdvancedBoxRoomParameters // TODO:
    {
        internal uint         abrp_Size;                               ///< Size of struct
        // public AudioBands       abrp_ReflectLeft, abrp_ReflectRight;     ///< Reflection bands (0 - 1.0)
        // public AudioBands       abrp_ReflectUp, abrp_ReflectDown;        ///< Reflection bands (0 - 1.0)
        // public AudioBands       abrp_ReflectBehind, abrp_ReflectFront;   ///< Reflection bands (0 - 1.0)
        public float            abrp_Width, abrp_Height, abrp_Depth;     ///< Size of box in meters
        public int              abrp_LockToListenerPosition;             ///< Whether box is centered on listener
        // public ovrAudioVector3f abrp_RoomPosition;
    }

    public enum ovrAudioMaterialPreset
    {
        AcousticTile,
        Brick,
        BrickPainted,
        Carpet,
        CarpetHeavy,
        CarpetHeavyPadded,
        CeramicTile,
        Concrete,
        ConcreteRough,
        ConcreteBlock,
        ConcreteBlockPainted,
        Curtain,
        Foliage,
        Glass,
        GlassHeavy,
        Grass,
        Gravel,
        GypsumBoard,
        PlasterOnBrick,
        PlasterOnConcreteBlock,
        Soil,
        SoundProof,
        Snow,
        Steel,
        Water,
        WoodThin,
        WoodThick,
        WoodFloor,
        WoodOnConcrete,
        COUNT
    }

    public static partial class ovrAudio
    {
#if UNITY_64 || X64
        internal const string DLL_PATH = "ovraudio64";
#else
        internal const string DLL_PATH = "ovraudio32";
#endif


        public const ovrResult ovrSuccess = 0;

        public const int MAJOR_VERSION = 1;
        public const int MINOR_VERSION = 64;
        public const int PATCH_VERSION = 0;

        public static string GetVersion(out int major, out int minor, out int patch)
        {
            return PtrToStringUtf8(ovrAudio_GetVersion(out major, out minor, out patch));
        }
        [DllImport(DLL_PATH)]
        internal static extern IntPtr ovrAudio_GetVersion([Out] out int Major, [Out] out int Minor, [Out] out int Patch);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AllocSamples")]
        public static extern IntPtr AllocSamples(int NumSamples);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_FreeSamples")]
        public static extern void FreeSamples(IntPtr Samples);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetTransformFromPose")]
        public static extern ovrResult GetTransformFromPose(    [In] ref ovrPosef Pose,
                                                                [Out] out float Vx, [Out] out float Vy, [Out] out float Vz,
                                                                [Out] out float Pos);

        public static ovrResult CreateContext(out ovrAudioContext Context, ref ovrAudioContextConfiguration Config)
        {
            Config.acc_Size = (uint)Marshal.SizeOf<ovrAudioContextConfiguration>();
            return ovrAudio_CreateContext(out Context, ref Config);
        }
        [DllImport(DLL_PATH)]
        internal static extern ovrResult ovrAudio_CreateContext([Out] out ovrAudioContext Context, [In] ref ovrAudioContextConfiguration pConfig);

        public static ovrResult InitializeContext(ovrAudioContext Context, ref ovrAudioContextConfiguration Config)
        {
            Config.acc_Size = (uint)Marshal.SizeOf<ovrAudioContextConfiguration>();
            return ovrAudio_InitializeContext(Context, ref Config);
        }
        [DllImport(DLL_PATH)]
        internal static extern ovrResult ovrAudio_InitializeContext([In] ovrAudioContext Context, [In] ref ovrAudioContextConfiguration pConfig);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_DestroyContext")]
        public static extern void DestroyContext([In] ovrAudioContext Context);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_Enable")]
        public static extern ovrResult Enable([In] ovrAudioContext Context, [In] ovrAudioEnable What, [In] int Enable);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_IsEnabled")]
        public static extern ovrResult IsEnabled([In] ovrAudioContext Context, [In] ovrAudioEnable What, [Out] out int Enabled);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetUnitScale")]
        public static extern ovrResult SetUnitScale([In] ovrAudioContext Context, [In] float UnitScale);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetUnitScale")]
        public static extern ovrResult GetUnitScale([In] ovrAudioContext Context, [Out] out float UnitScale);

        /// Set HRTF interpolation method.
        ///
        /// NOTE: Internal use only!
        /// \param Context context to use
        /// \param InterpolationMethod method to use
        ///
        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetHRTFInterpolationMethod")]
        public static extern ovrResult SetHRTFInterpolationMethod([In] ovrAudioContext Context, [In] ovrAudioHRTFInterpolationMethod InterpolationMethod);

        /// Get HRTF interpolation method.
        ///
        /// NOTE: Internal use only!
        /// \param Context context to use
        /// \param InterpolationMethod method to use
        ///
        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetHRTFInterpolationMethod")]
        public static extern ovrResult GetHRTFInterpolationMethod([In] ovrAudioContext Context, [Out] out ovrAudioHRTFInterpolationMethod InterpolationMethod);

        public static ovrResult SetSimpleBoxRoomParameters(ovrAudioContext Context, ref ovrAudioBoxRoomParameters Parameters)
        {
            Parameters.brp_Size = (uint)Marshal.SizeOf<ovrAudioBoxRoomParameters>();
            return ovrAudio_SetSimpleBoxRoomParameters(Context, ref Parameters);
        }
        [DllImport(DLL_PATH)]
        internal static extern ovrResult ovrAudio_SetSimpleBoxRoomParameters([In] ovrAudioContext Context, [In] ref ovrAudioBoxRoomParameters Parameters);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetSimpleBoxRoomParameters")]
        public static extern ovrResult GetSimpleBoxRoomParameters([In] ovrAudioContext Context, [Out] out ovrAudioBoxRoomParameters Parameters);


        // TODO: add advanced room methods here


        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetListenerVectors")]
        public static extern ovrResult SetListenerVectors(  [In] ovrAudioContext Context,
                                                            [In] float PositionX, [In] float PositionY, [In] float PositionZ,
                                                            [In] float ForwardX, [In] float ForwardY, [In] float ForwardZ,
                                                            [In] float UpX, [In] float UpY, [In] float UpZ);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetListenerVectors")]
        public static extern ovrResult GetListenerVectors([In] ovrAudioContext Context,
                                                            [Out] out float PositionX, [Out] out float PositionY, [Out] out float PositionZ,
                                                            [Out] out float ForwardX, [Out] out float ForwardY, [Out] out float ForwardZ,
                                                            [Out] out float UpX, [Out] out float UpY, [Out] out float UpZ);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_ResetAudioSource")]
        public static extern ovrResult ResetAudioSource([In] ovrAudioContext Context, [In] int Sound);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetAudioSourcePos")]
        public static extern ovrResult SetAudioSourcePos([In] ovrAudioContext Context, [In] int Sound, [In] float X, [In] float Y, [In] float Z);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetAudioSourcePos")]
        public static extern ovrResult GetAudioSourcePos([In] ovrAudioContext Context, int Sound, [Out] out float X, [Out] out float Y, [Out] out float Z);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetAudioSourceRange")]
        public static extern ovrResult SetAudioSourceRange([In] ovrAudioContext Context, [In] int Sound, [In] float RangeMin, [In] float RangeMax);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetAudioSourceRange")]
        public static extern ovrResult GetAudioSourceRange([In] ovrAudioContext Context, [In] int Sound, [Out] out float RangeMin, [Out] out float pRangeMax);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetAudioSourceRadius")]
        public static extern ovrResult SetAudioSourceRadius([In] ovrAudioContext Context, [In] int Sound, [In] float Radius);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetAudioSourceRadius")]
        public static extern ovrResult GetAudioSourceRadius([In] ovrAudioContext Context, [In] int Sound, [Out] out float Radius);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetAudioReverbSendLevel")]
        public static extern ovrResult SetAudioReverbSendLevel([In] ovrAudioContext Context, [In] int Sound, [In] float Level);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetAudioReverbSendLevel")]
        public static extern ovrResult GetAudioReverbSendLevel([In] ovrAudioContext Context, [In] int Sound, [Out] out float Level);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetAudioSourceFlags")]
        public static extern ovrResult SetAudioSourceFlags([In] ovrAudioContext Context, [In] int Sound, [In] uint Flags);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetAudioSourceFlags")]
        public static extern ovrResult GetAudioSourceFlags([In] ovrAudioContext Context, [In] int Sound, [Out] out uint Flags);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetAudioSourceAttenuationMode")]
        public static extern ovrResult SetAudioSourceAttenuationMode([In] ovrAudioContext Context, [In] int Sound, [In] ovrAudioSourceAttenuationMode Mode, [In] float SourceGain);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetAudioSourceAttenuationMode")]
        public static extern ovrResult GetAudioSourceAttenuationMode([In] ovrAudioContext Context, [In] int Sound, [Out] out ovrAudioSourceAttenuationMode Mode, [Out] out float pSourceGain);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetAudioSourceOverallGain")]
        public static extern ovrResult GetAudioSourceOverallGain([In] ovrAudioContext Context, [In] int Sound, [Out] out float Gain);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SpatializeMonoSourceInterleaved")]
        public static unsafe extern ovrResult SpatializeMonoSourceInterleaved([In] ovrAudioContext Context, [In] int Sound, [Out] out uint OutStatus, float * Dst, float * Src);
                
        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SpatializeMonoSourceLR")]
        public static unsafe extern ovrResult SpatializeMonoSourceLR([In] ovrAudioContext Context, [In] int Sound, [Out] out uint OutStatus, float * DstLeft, float * DstRight, float * Src);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_MixInSharedReverbLR")]
        public static unsafe extern ovrResult MixInSharedReverbLR([In] ovrAudioContext Context, [Out] out uint OutStatus, float * DstLeft, float * DstRight);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_MixInSharedReverbLR")]
        public static unsafe extern ovrResult MixInSharedReverbLR([In] ovrAudioContext Context, [Out] out uint *OutStatus, float * DstLeft, float * DstRight);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_MixInSharedReverbInterleaved")]
        public static unsafe extern ovrResult MixInSharedReverbInterleaved([In] ovrAudioContext Context, [Out] out uint OutStatus, float * DstInterleaved);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetSharedReverbWetLevel")]
        public static extern ovrResult SetSharedReverbWetLevel([In] ovrAudioContext Context, [In] float Level);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetSharedReverbWetLevel")]
        public static extern ovrResult GetSharedReverbWetLevel([In] ovrAudioContext Context, [Out] out float Level);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetHeadRadius")]
        public static extern ovrResult SetHeadRadius([In] ovrAudioContext Context, [In] float HeadRadius );

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetHeadRadius")]
        public static extern ovrResult GetHeadRadius([In] ovrAudioContext Context, [Out] out float HeadRadius);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetPerformanceCounter")]
        public static unsafe extern ovrResult GetPerformanceCounter([In] ovrAudioContext Context, [In] ovrAudioPerformanceCounter Counter, [Out] out Int64 Count, out double TimeMicroSeconds);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_ResetPerformanceCounter")]
        public static extern ovrResult ResetPerformanceCounter(ovrAudioContext Context, ovrAudioPerformanceCounter Counter);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_ProcessQuadBinaural")]
        public static unsafe extern ovrResult ProcessQuadBinaural(float * ForwardLR, float * RightLR, float * BackLR, float * LeftLR, [In] float LookDirectionX, [In] float LookDirectionY, [In] float LookDirectionZ, [In] int NumSamples, float * Dst);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_CreateAmbisonicStream")]
        public static extern ovrResult CreateAmbisonicStream([In] ovrAudioContext Context, [In] int SampleRate, [In] int AudioBufferLength, [In] ovrAudioAmbisonicFormat format, [In] int ambisonicOrder, [Out] out ovrAudioAmbisonicStream pAmbisonicStream);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_ResetAmbisonicStream")]
        public static extern ovrResult ResetAmbisonicStream([In] ovrAudioAmbisonicStream AmbisonicStream);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_DestroyAmbisonicStream")]
        public static extern ovrResult DestroyAmbisonicStream([In] ovrAudioAmbisonicStream AmbisonicStream);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetAmbisonicRenderMode")]
        public static extern ovrResult SetAmbisonicRenderMode([In] ovrAudioAmbisonicStream AmbisonicStream, [In] ovrAudioAmbisonicRenderMode Mode);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetAmbisonicRenderMode")]
        public static extern ovrResult GetAmbisonicRenderMode([In] ovrAudioAmbisonicStream AmbisonicStream, [Out] out ovrAudioAmbisonicRenderMode Mode);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_MonoToAmbisonic")]
        public static unsafe extern ovrResult MonoToAmbisonic(float * InMono, [In] float DirectionX, [In] float DirectionY, [In] float DirectionZ, [In] ovrAudioAmbisonicFormat Format, [In] int AmbisonicOrder, float * OutAmbisonic, [In] int NumSamples);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_ProcessAmbisonicStreamInterleaved")]
        public static unsafe extern ovrResult ProcessAmbisonicStreamInterleaved([In] ovrAudioContext Context, [In] ovrAudioAmbisonicStream AmbisonicStream, float * Src, float * Dst, [In] int NumSamples);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetAmbisonicOrientation")]
        public static extern ovrResult SetAmbisonicOrientation([In] ovrAudioAmbisonicStream AmbisonicStream, [In] float LookDirectionX, [In] float LookDirectionY, [In] float LookDirectionZ, [In] float UpDirectionX, [In] float UpDirectionY, [In] float UpDirectionZ);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetAmbisonicOrientation")]
        public static extern ovrResult GetAmbisonicOrientation([In] ovrAudioAmbisonicStream AmbisonicStream, [Out] out float LookDirectionX, [Out] out float LookDirectionY, [Out] out float LookDirectionZ, [Out] out float UpDirectionX, [Out] out float UpDirectionY, [Out] out float UpDirectionZ);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetProfilerEnabled")]
        public static extern ovrResult SetProfilerEnabled([In] ovrAudioContext Context, [In] int Enabled);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetProfilerPort")]
        public static extern ovrResult SetProfilerPort([In] ovrAudioContext Context, [In] int Port);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetReflectionModel")]
        public static extern ovrResult SetReflectionModel([In] ovrAudioContext Context, [In] ovrAudioReflectionModel Model);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AssignRaycastCallback")]
        public static extern ovrResult AssignRaycastCallback(ovrAudioContext Context, OVRA_RAYCAST_CALLBACK Callback, IntPtr pctx);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetDynamicRoomRaysPerSecond")]
        public static extern ovrResult SetDynamicRoomRaysPerSecond([In] ovrAudioContext Context, [In] int RaysPerSecond);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetDynamicRoomInterpSpeed")]
        public static extern ovrResult SetDynamicRoomInterpSpeed([In] ovrAudioContext Context, [In] float InterpSpeed);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetDynamicRoomMaxWallDistance")]
        public static extern ovrResult SetDynamicRoomMaxWallDistance([In] ovrAudioContext Context, [In] float MaxWallDistance);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetDynamicRoomRaysRayCacheSize")]
        public static extern ovrResult SetDynamicRoomRaysRayCacheSize([In] ovrAudioContext Context, [In] int RayCacheSize);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetRoomDimensions")]
        public static extern ovrResult GetRoomDimensions([In] ovrAudioContext Context, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] float[] RoomDimensions, [MarshalAs(UnmanagedType.LPArray, SizeConst = 6)] float[] ReflectionsCoefs, [Out] out ovrAudioVector3f Position);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_GetRaycastHits")]
        public static extern ovrResult GetRaycastHits([In] ovrAudioContext Context, [MarshalAs(UnmanagedType.LPArray)] ovrAudioVector3f[] Points, [MarshalAs(UnmanagedType.LPArray)] ovrAudioVector3f[] Normals, int Length);

        // Propagation is only supported on Windows
        // All methods below will return ovrError_AudioUnsupportedFeature on other platforms

        /***********************************************************************************/
        /* Geometry API */
        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetPropagationQuality")]
        public static extern ovrResult SetPropagationQuality([In] ovrAudioContext context, [In] float quality);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_SetPropagationThreadAffinity")]
        public static extern ovrResult SetPropagationThreadAffinity([In] ovrAudioContext context, [In] ulong cpuMask);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_CreateAudioGeometry")]
        public static extern ovrResult CreateAudioGeometry([In] ovrAudioContext context, out ovrAudioGeometry geometry);
        
        [DllImport(DLL_PATH, EntryPoint="ovrAudio_DestroyAudioGeometry")]
        public static extern ovrResult DestroyAudioGeometry([In] ovrAudioGeometry geometry);
        
        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AudioGeometryUploadMesh")]
        public static extern ovrResult AudioGeometryUploadMesh([In] ovrAudioGeometry geometry, [In] ref ovrAudioMesh mesh);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AudioGeometryUploadMesh")]
        public static unsafe extern ovrResult AudioGeometryUploadMeshArrays([In] ovrAudioGeometry geometry, IntPtr vertices, [In] size_t verticesByteOffset, [In] size_t vertexCount, [In] size_t vertexStride, [In] ovrAudioScalarType vertexType, IntPtr indices, [In] size_t indicesByteOffset, [In] size_t indexCount, [In] ovrAudioScalarType indexType, [In] ovrAudioMeshGroup * groups, size_t groupCount);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AudioGeometrySetTransform")]
        public static extern ovrResult AudioGeometrySetTransform([In] ovrAudioGeometry geometry, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] float[] matrix4x4);

        [DllImport(DLL_PATH, EntryPoint="DestroyAudioMaterial")]
        public static extern ovrResult ovrAudio_DestroyAudioMaterial([In] ovrAudioMaterial material);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AudioMaterialSetFrequency")]
        public static extern ovrResult AudioMaterialSetFrequency([In] ovrAudioMaterial material, [In] ovrAudioMaterialProperty property, [In] float frequency, [In] float value);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AudioMaterialGetFrequency")]
        public static extern ovrResult AudioMaterialGetFrequency([In] ovrAudioMaterial material, [In] ovrAudioMaterialProperty property, [In] float frequency, [Out] out float value);

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AudioMaterialReset")]
        public static extern ovrResult AudioMaterialReset([In] ovrAudioMaterial material, [In] ovrAudioMaterialProperty property);

        /***********************************************************************************/
        /* Serialization API */

        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AudioGeometryWriteMeshData")]
        public static extern ovrResult AudioGeometryWriteMeshData([In] ovrAudioGeometry geometry, [In] ref ovrAudioSerializer serializer);
        
        [DllImport(DLL_PATH, EntryPoint="ovrAudio_AudioGeometryReadMeshData")]
        public static extern ovrResult AudioGeometryReadMeshData([In] ovrAudioGeometry geometry, ref ovrAudioSerializer serializer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ovrResult GetReflectionBands(ovrAudioMaterialPreset Preset, float[] Bands)
        {
            if ((int)Preset >= (int)ovrAudioMaterialPreset.COUNT || Bands == null || Bands.Length != 4)
            {
                return (int)ovrAudioError.AudioInvalidParam;
            }

            switch (Preset)
            {
            case ovrAudioMaterialPreset.AcousticTile:
                Bands[0] = 0.488168418f;
                Bands[1] = 0.361475229f;
                Bands[2] = 0.339595377f;
                Bands[3] = 0.498946249f;
                break;
            case ovrAudioMaterialPreset.Brick:
                Bands[0] = 0.975468814f;
                Bands[1] = 0.972064495f;
                Bands[2] = 0.949180186f;
                Bands[3] = 0.930105388f;
                break;
            case ovrAudioMaterialPreset.BrickPainted:
                Bands[0] = 0.975710571f;
                Bands[1] = 0.983324170f;
                Bands[2] = 0.978116691f;
                Bands[3] = 0.970052719f;
                break;
            case ovrAudioMaterialPreset.Carpet:
                Bands[0] = 0.987633705f;
                Bands[1] = 0.905486643f;
                Bands[2] = 0.583110571f;
                Bands[3] = 0.351053834f;
                break;
            case ovrAudioMaterialPreset.CarpetHeavy:
                Bands[0] = 0.977633715f;
                Bands[1] = 0.859082878f;
                Bands[2] = 0.526479602f;
                Bands[3] = 0.370790422f;
                break;
            case ovrAudioMaterialPreset.CarpetHeavyPadded:
                Bands[0] = 0.910534739f;
                Bands[1] = 0.530433178f;
                Bands[2] = 0.294055820f;
                Bands[3] = 0.270105422f;
                break;
            case ovrAudioMaterialPreset.CeramicTile:
                Bands[0] = 0.990000010f;
                Bands[1] = 0.990000010f;
                Bands[2] = 0.982753932f;
                Bands[3] = 0.980000019f;
                break;
            case ovrAudioMaterialPreset.Concrete:
                Bands[0] = 0.990000010f;
                Bands[1] = 0.983324170f;
                Bands[2] = 0.980000019f;
                Bands[3] = 0.980000019f;
                break;
            case ovrAudioMaterialPreset.ConcreteRough:
                Bands[0] = 0.989408433f;
                Bands[1] = 0.964494646f;
                Bands[2] = 0.922127008f;
                Bands[3] = 0.900105357f;
                break;
            case ovrAudioMaterialPreset.ConcreteBlock:
                Bands[0] = 0.635267377f;
                Bands[1] = 0.652230680f;
                Bands[2] = 0.671053469f;
                Bands[3] = 0.789051592f;
                break;
            case ovrAudioMaterialPreset.ConcreteBlockPainted:
                Bands[0] = 0.902957916f;
                Bands[1] = 0.940235913f;
                Bands[2] = 0.917584062f;
                Bands[3] = 0.919947326f;
                break;
            case ovrAudioMaterialPreset.Curtain:
                Bands[0] = 0.686494231f;
                Bands[1] = 0.545859993f;
                Bands[2] = 0.310078561f;
                Bands[3] = 0.399473131f;
                break;
            case ovrAudioMaterialPreset.Foliage:
                Bands[0] = 0.518259346f;
                Bands[1] = 0.503568292f;
                Bands[2] = 0.578688800f;
                Bands[3] = 0.690210819f;
                break;
            case ovrAudioMaterialPreset.Glass:
                Bands[0] = 0.655915797f;
                Bands[1] = 0.800631821f;
                Bands[2] = 0.918839693f;
                Bands[3] = 0.923488140f;
                break;
            case ovrAudioMaterialPreset.GlassHeavy:
                Bands[0] = 0.827098966f;
                Bands[1] = 0.950222731f;
                Bands[2] = 0.974604130f;
                Bands[3] = 0.980000019f;
                break;
            case ovrAudioMaterialPreset.Grass:
                Bands[0] = 0.881126285f;
                Bands[1] = 0.507170796f;
                Bands[2] = 0.131893098f;
                Bands[3] = 0.0103688836f;
                break;
            case ovrAudioMaterialPreset.Gravel:
                Bands[0] = 0.729294717f;
                Bands[1] = 0.373122454f;
                Bands[2] = 0.255317450f;
                Bands[3] = 0.200263441f;
                break;
            case ovrAudioMaterialPreset.GypsumBoard:
                Bands[0] = 0.721240044f;
                Bands[1] = 0.927690148f;
                Bands[2] = 0.934302270f;
                Bands[3] = 0.910105407f;
                break;
            case ovrAudioMaterialPreset.PlasterOnBrick:
                Bands[0] = 0.975696504f;
                Bands[1] = 0.979106009f;
                Bands[2] = 0.961063504f;
                Bands[3] = 0.950052679f;
                break;
            case ovrAudioMaterialPreset.PlasterOnConcreteBlock:
                Bands[0] = 0.881774724f;
                Bands[1] = 0.924773932f;
                Bands[2] = 0.951497555f;
                Bands[3] = 0.959947288f;
                break;
            case ovrAudioMaterialPreset.Soil:
                Bands[0] = 0.844084203f;
                Bands[1] = 0.634624243f;
                Bands[2] = 0.416662872f;
                Bands[3] = 0.400000036f;
                break;
            case ovrAudioMaterialPreset.SoundProof:
                Bands[0] = 0.000000000f;
                Bands[1] = 0.000000000f;
                Bands[2] = 0.000000000f;
                Bands[3] = 0.000000000f;
                break;
            case ovrAudioMaterialPreset.Snow:
                Bands[0] = 0.532252669f;
                Bands[1] = 0.154535770f;
                Bands[2] = 0.0509644151f;
                Bands[3] = 0.0500000119f;
                break;
            case ovrAudioMaterialPreset.Steel:
                Bands[0] = 0.793111682f;
                Bands[1] = 0.840140402f;
                Bands[2] = 0.925591767f;
                Bands[3] = 0.979736567f;
                break;
            case ovrAudioMaterialPreset.Water:
                Bands[0] = 0.970588267f;
                Bands[1] = 0.971753478f;
                Bands[2] = 0.978309572f;
                Bands[3] = 0.970052719f;
                break;
            case ovrAudioMaterialPreset.WoodThin:
                Bands[0] = 0.592423141f;
                Bands[1] = 0.858273327f;
                Bands[2] = 0.917242289f;
                Bands[3] = 0.939999998f;
                break;
            case ovrAudioMaterialPreset.WoodThick:
                Bands[0] = 0.812957883f;
                Bands[1] = 0.895329595f;
                Bands[2] = 0.941304684f;
                Bands[3] = 0.949947298f;
                break;
            case ovrAudioMaterialPreset.WoodFloor:
                Bands[0] = 0.852366328f;
                Bands[1] = 0.898992121f;
                Bands[2] = 0.934784114f;
                Bands[3] = 0.930052698f;
                break;
            case ovrAudioMaterialPreset.WoodOnConcrete:
                Bands[0] = 0.959999979f;
                Bands[1] = 0.941232264f;
                Bands[2] = 0.937923789f;
                Bands[3] = 0.930052698f;
                break;
            default:
                Bands[0] = 0.000000000f;
                Bands[1] = 0.000000000f;
                Bands[2] = 0.000000000f;
                Bands[3] = 0.000000000f;
                return (int)ovrAudioError.AudioInvalidParam;
            };

            return ovrSuccess;
        }

#region Helpers
        private static string PtrToStringUtf8(IntPtr ptr) // aPtr is nul-terminated
        {
            if (ptr == IntPtr.Zero)
                return "";
            int len = 0;
            while (System.Runtime.InteropServices.Marshal.ReadByte(ptr, len) != 0)
                len++;
            if (len == 0)
                return "";
            byte[] array = new byte[len];
            System.Runtime.InteropServices.Marshal.Copy(ptr, array, 0, len);
            return System.Text.Encoding.UTF8.GetString(array);
        }
    }
#endregion
}
