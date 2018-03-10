﻿using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Services
{
    public class GameService : ServiceBase<GameOperations, GameEvents>, IGameServiceAPI
    {
        public UnityEvent<EnterSceneResponseParameters> SceneEntered { get; } = new UnityEvent<EnterSceneResponseParameters>();
        public UnityEvent<SceneObjectAddedEventParameters> SceneObjectAdded { get; } = new UnityEvent<SceneObjectAddedEventParameters>();
        public UnityEvent<SceneObjectRemovedEventParameters> SceneObjectRemoved { get; } = new UnityEvent<SceneObjectRemovedEventParameters>();
        public UnityEvent<SceneObjectsAddedEventParameters> SceneObjectsAdded { get; } = new UnityEvent<SceneObjectsAddedEventParameters>();
        public UnityEvent<SceneObjectsRemovedEventParameters> SceneObjectsRemoved { get; } = new UnityEvent<SceneObjectsRemovedEventParameters>();
        public UnityEvent<SceneObjectPositionChangedEventParameters> PositionChanged { get; } = new UnityEvent<SceneObjectPositionChangedEventParameters>();
        public UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; } = new UnityEvent<PlayerStateChangedEventParameters>();
        public UnityEvent<PlayerAttackedEventParameters> PlayerAttacked { get; } = new UnityEvent<PlayerAttackedEventParameters>();
        public UnityEvent<CharacterAddedEventParameters> CharacterAdded { get; } = new UnityEvent<CharacterAddedEventParameters>();
        public UnityEvent<CharactersAddedEventParameters> CharactersAdded { get; } = new UnityEvent<CharactersAddedEventParameters>();

        protected override void OnConnected()
        {
            base.OnConnected();

            SetEventsHandlers();
        }

        protected override void OnDisconnected(DisconnectReason reason, string details)
        {
            base.OnDisconnected(reason, details);

            RemoveEventsHandlers();
        }

        private void SetEventsHandlers()
        {
            ServerPeerHandler.SetEventHandler((byte)GameEvents.SceneObjectAdded, SceneObjectAdded);
            ServerPeerHandler.SetEventHandler((byte)GameEvents.SceneObjectRemoved, SceneObjectRemoved);
            ServerPeerHandler.SetEventHandler((byte)GameEvents.SceneObjectsAdded, SceneObjectsAdded);
            ServerPeerHandler.SetEventHandler((byte)GameEvents.SceneObjectsRemoved, SceneObjectsRemoved);
            ServerPeerHandler.SetEventHandler((byte)GameEvents.PositionChanged, PositionChanged);
            ServerPeerHandler.SetEventHandler((byte)GameEvents.PlayerStateChanged, PlayerStateChanged);
            ServerPeerHandler.SetEventHandler((byte)GameEvents.PlayerAttacked, PlayerAttacked);
            ServerPeerHandler.SetEventHandler((byte)GameEvents.CharacterAdded, CharacterAdded);
            ServerPeerHandler.SetEventHandler((byte)GameEvents.CharactersAdded, CharactersAdded);
        }

        private void RemoveEventsHandlers()
        {
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.SceneObjectAdded);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.SceneObjectRemoved);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.SceneObjectsAdded);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.SceneObjectsRemoved);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.PositionChanged);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.PlayerStateChanged);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.PlayerAttacked);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.CharacterAdded);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.CharactersAdded);
        }

        public async Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<AuthorizeRequestParameters, AuthorizeResponseParameters>
                (yield, (byte)GameOperations.Authorize, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task EnterScene(IYield yield)
        {
            var parameters = new EmptyParameters();
            var responseParameters = await ServerPeerHandler.SendOperation<EmptyParameters, EnterSceneResponseParameters>
                (yield, (byte)GameOperations.EnterScene, parameters, MessageSendOptions.DefaultReliable());

            SceneEntered?.Invoke(responseParameters);
        }

        public async Task<ChangeSceneResponseParameters> ChangeScene(IYield yield, ChangeSceneRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<ChangeSceneRequestParameters, ChangeSceneResponseParameters>
                (yield, (byte)GameOperations.ChangeScene, parameters, MessageSendOptions.DefaultReliable());
        }

        public void UpdatePosition(UpdatePositionRequestParameters parameters) =>
            ServerPeerHandler.SendOperation((byte)GameOperations.PositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));

        public void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters) =>
            ServerPeerHandler.SendOperation((byte)GameOperations.PlayerStateChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations));
    }
}