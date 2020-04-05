﻿using UnityEngine;
using System.Collections.Generic;

namespace com.spacepuppy.Utils
{

    public static class CoroutineUtil
    {

        #region StartCoroutine

        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Collections.IEnumerable enumerable)
        {
            if (behaviour == null) throw new System.ArgumentNullException("behaviour");
            return behaviour.StartCoroutine(enumerable.GetEnumerator());
        }

        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Func<System.Collections.IEnumerator> method)
        {
            if (behaviour == null) throw new System.ArgumentNullException("behaviour");
            if (method == null) throw new System.ArgumentNullException("method");

            return behaviour.StartCoroutine(method());
        }

        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Delegate method, params object[] args)
        {
            if (behaviour == null) throw new System.ArgumentNullException("behaviour");
            if (method == null) throw new System.ArgumentNullException("method");

            System.Collections.IEnumerator e;
            if (com.spacepuppy.Utils.TypeUtil.IsType(method.Method.ReturnType, typeof(System.Collections.IEnumerable)))
            {
                e = (method.DynamicInvoke(args) as System.Collections.IEnumerable).GetEnumerator();
            }
            else if (com.spacepuppy.Utils.TypeUtil.IsType(method.Method.ReturnType, typeof(System.Collections.IEnumerator)))
            {
                e = (method.DynamicInvoke(args) as System.Collections.IEnumerator);
            }
            else
            {
                throw new System.ArgumentException("Delegate must have a return type of IEnumerable or IEnumerator.", "method");
            }

            return behaviour.StartCoroutine(e);
        }

        #endregion

        #region Invoke

        public static Coroutine InvokeLegacy(this MonoBehaviour behaviour, System.Action method, float delay)
        {
            if (behaviour == null) throw new System.ArgumentNullException("behaviour");
            if (method == null) throw new System.ArgumentNullException("method");

            return behaviour.StartCoroutine(InvokeRedirect(method, delay));
        }

        //public static RadicalCoroutine Invoke(this MonoBehaviour behaviour, System.Action method, float delay, ITimeSupplier time = null, RadicalCoroutineDisableMode disableMode = RadicalCoroutineDisableMode.CancelOnDisable)
        //{
        //    if (behaviour == null) throw new System.ArgumentNullException("behaviour");
        //    if (method == null) throw new System.ArgumentNullException("method");

        //    return StartRadicalCoroutine(behaviour, RadicalInvokeRedirect(method, delay, -1f, time), disableMode);
        //}

        //public static InvokeHandle InvokeGuaranteed(this MonoBehaviour behaviour, System.Action method, float delay, ITimeSupplier time = null)
        //{
        //    if (method == null) throw new System.ArgumentNullException("method");
        //    //return StartRadicalCoroutine(GameLoop.Hook, RadicalInvokeRedirect(method, delay, -1f, time));

        //    return InvokeHandle.Begin(GameLoop.UpdatePump, method, delay, time);
        //}

        //public static RadicalCoroutine InvokeRepeating(this MonoBehaviour behaviour, System.Action method, float delay, float repeatRate, ITimeSupplier time = null, RadicalCoroutineDisableMode disableMode = RadicalCoroutineDisableMode.CancelOnDisable)
        //{
        //    if (behaviour == null) throw new System.ArgumentNullException("behaviour");
        //    if (method == null) throw new System.ArgumentNullException("method");

        //    return StartRadicalCoroutine(behaviour, RadicalInvokeRedirect(method, delay, repeatRate, time), disableMode);
        //}



        private static System.Collections.IEnumerator InvokeRedirect(System.Action method, float delay, float repeatRate = -1f)
        {
            yield return new WaitForSeconds(delay);

            if (repeatRate < 0f)
            {
                method();
            }
            else if (repeatRate == 0f)
            {
                while (true)
                {
                    method();
                    yield return null;
                }
            }
            else
            {
                var w = new WaitForSeconds(repeatRate);
                while (true)
                {
                    method();
                    yield return w;
                }
            }
        }

        //internal static System.Collections.IEnumerator RadicalInvokeRedirect(System.Action method, float delay, float repeatRate = -1f, ITimeSupplier time = null)
        //{
        //    if (delay < SPConstants.MIN_FRAME_DELTA)
        //        yield return null;
        //    else if (delay > 0f)
        //        yield return WaitForDuration.Seconds(delay, time);

        //    if (repeatRate < 0f)
        //    {
        //        method();
        //    }
        //    else if (repeatRate == 0f)
        //    {
        //        while (true)
        //        {
        //            method();
        //            yield return null;
        //        }
        //    }
        //    else
        //    {
        //        while (true)
        //        {
        //            method();
        //            yield return WaitForDuration.Seconds(repeatRate, time);
        //        }
        //    }
        //}

        //public static RadicalCoroutine InvokeAfterYield(this MonoBehaviour behaviour, System.Action method, object yieldInstruction, RadicalCoroutineDisableMode disableMode = RadicalCoroutineDisableMode.CancelOnDisable)
        //{
        //    if (behaviour == null) throw new System.ArgumentNullException("behaviour");
        //    if (method == null) throw new System.ArgumentNullException("method");

        //    return StartRadicalCoroutine(behaviour, InvokeAfterYieldRedirect(method, yieldInstruction));
        //}

        //internal static System.Collections.IEnumerator InvokeAfterYieldRedirect(System.Action method, object yieldInstruction)
        //{
        //    yield return yieldInstruction;
        //    method();
        //}

        #endregion

    }

}