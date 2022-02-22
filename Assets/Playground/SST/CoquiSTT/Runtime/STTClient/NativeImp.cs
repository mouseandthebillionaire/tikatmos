﻿using STTClient.Enums;

using System;
using System.Runtime.InteropServices;

namespace STTClient
{
    /// <summary>
    /// Wrapper for the native implementation of "libstt.so"
    /// </summary>
    internal static class NativeImp
    {
        #region Native Implementation
        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern IntPtr STT_Version();

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern ErrorCodes STT_CreateModel(string aModelPath,
            ref IntPtr** pint);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern IntPtr STT_ErrorCodeToErrorMessage(int aErrorCode);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern uint STT_GetModelBeamWidth(IntPtr** aCtx);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern ErrorCodes STT_SetModelBeamWidth(IntPtr** aCtx,
            uint aBeamWidth);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern ErrorCodes STT_CreateModel(string aModelPath,
            uint aBeamWidth,
            ref IntPtr** pint);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern int STT_GetModelSampleRate(IntPtr** aCtx);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern ErrorCodes STT_EnableExternalScorer(IntPtr** aCtx,
            string aScorerPath);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern ErrorCodes STT_AddHotWord(IntPtr** aCtx,
            string aWord,
            float aBoost);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern ErrorCodes STT_EraseHotWord(IntPtr** aCtx,
            string aWord);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern ErrorCodes STT_ClearHotWords(IntPtr** aCtx);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern ErrorCodes STT_DisableExternalScorer(IntPtr** aCtx);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern ErrorCodes STT_SetScorerAlphaBeta(IntPtr** aCtx,
            float aAlpha,
            float aBeta);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi, SetLastError = true)]
        internal static unsafe extern IntPtr STT_SpeechToText(IntPtr** aCtx,
            short[] aBuffer,
            uint aBufferSize);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static unsafe extern IntPtr STT_SpeechToTextWithMetadata(IntPtr** aCtx,
            short[] aBuffer,
            uint aBufferSize,
            uint aNumResults);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern void STT_FreeModel(IntPtr** aCtx);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern ErrorCodes STT_CreateStream(IntPtr** aCtx,
               ref IntPtr** retval);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern void STT_FreeStream(IntPtr** aSctx);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern void STT_FreeMetadata(IntPtr metadata);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern void STT_FreeString(IntPtr str);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi, SetLastError = true)]
        internal static unsafe extern void STT_FeedAudioContent(IntPtr** aSctx,
            short[] aBuffer,
            uint aBufferSize);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern IntPtr STT_IntermediateDecode(IntPtr** aSctx);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern IntPtr STT_IntermediateDecodeWithMetadata(IntPtr** aSctx,
            uint aNumResults);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi, SetLastError = true)]
        internal static unsafe extern IntPtr STT_FinishStream(IntPtr** aSctx);

        [DllImport("libstt.so", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern IntPtr STT_FinishStreamWithMetadata(IntPtr** aSctx,
            uint aNumResults);
        #endregion
    }
}
