using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading;
using System.Web;

namespace WcfService1
{
    [DebuggerStepThrough]
    public class STAOperationBehaviorAttribute : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription,
            System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription,
            System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
            // If this is applied on the client, well, it just doesn’t make sense.
            // Don’t throw in case this attribute was applied on the contract
            // instead of the implementation.
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription,
            System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            // Change the IOperationInvoker for this operation.
            dispatchOperation.Invoker = new STAOperationInvoker(dispatchOperation.Invoker);
        }

        public void Validate(OperationDescription operationDescription)
        {
            if (operationDescription.SyncMethod == null)
            {
                throw new InvalidOperationException("The STAOperationBehaviorAttribute " +
                    "only works for synchronous method invocations.");
            }
        }
    }

    public class STAOperationInvoker : IOperationInvoker
    {
        IOperationInvoker _innerInvoker;
        public STAOperationInvoker(IOperationInvoker invoker)
        {
            _innerInvoker = invoker;
        }

        public object[] AllocateInputs()
        {
            return _innerInvoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            // Create a new, STA thread
            object[] staOutputs = null;
            object retval = null;
            Thread thread = new Thread(
                delegate ()
                {
                    retval = _innerInvoker.Invoke(instance, inputs, out staOutputs);
                });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            outputs = staOutputs;
            return retval;
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs,
            AsyncCallback callback, object state)
        {
            // We don’t handle async…
            throw new NotImplementedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            // We don’t handle async…
            throw new NotImplementedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }
}