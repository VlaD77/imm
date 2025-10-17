using System;
using System.Collections.Generic;

namespace imm.Core
{
	public class AsyncToken<T>
	{
		private readonly List<Responder<T>> _responders;
		
		private T _command;
		
		
		public AsyncToken (T command)
		{
			_responders = new List<Responder<T>>();
			_command = command;
		}
		
		public void AddResponder(Responder<T> responder)
		{
			_responders.Add(responder);
		}
		
		internal void FireResponse()
		{
			foreach(Responder<T> responder in _responders)
			{
				responder.result(_command);
			}
			
			_responders.Clear();
		}
		
		internal void FireFault()
		{
			foreach(Responder<T> responder in _responders)
			{
				if(responder.fault != null)
					responder.fault(_command);
			}
			
			_responders.Clear();			
		}
	}

	public sealed class AsyncTokenAdapter<TSource,TDestination>:AsyncToken<TDestination>
		where TSource:StateCommand
		where TDestination:class
	{
		public AsyncTokenAdapter(TSource command):base(command as TDestination)
		{
			command.AsyncToken.AddResponder(new Responder<StateCommand>(OnSuccess,OnFault));
		}

		private void OnSuccess(StateCommand command)
		{
			FireResponse();
		}
		
		private void OnFault(StateCommand command)
		{
			FireFault();
		}
	}


}

