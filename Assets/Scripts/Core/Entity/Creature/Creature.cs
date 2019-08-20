﻿using JetBrains.Annotations;
using UdpKit;
using UnityEngine;

namespace Core
{
    public class Creature : Unit
    {
        public new class CreateToken : Unit.CreateToken
        {
            public string CustomNameId = string.Empty;

            public override void Read(UdpPacket packet)
            {
                base.Read(packet);

                CustomNameId = packet.ReadString();
            }

            public override void Write(UdpPacket packet)
            {
                base.Write(packet);

                packet.WriteString(CustomNameId);
            }

            public void Attached(Creature creature)
            {
                base.Attached(creature);

                creature.Name = CustomNameId;
            }
        }

        [SerializeField, UsedImplicitly, Header(nameof(Creature)), Space(10)] private CreatureDefinition creatureDefinition;

        private CreateToken createToken;
        private string customNameId;

        internal override bool AutoScoped => true;

        public override string Name { get => string.IsNullOrEmpty(customNameId) ? creatureDefinition.CreatureNameId : customNameId; internal set => customNameId = value; }

        protected override void HandleAttach()
        {
            base.HandleAttach();

            createToken = (CreateToken)entity.AttachToken;
            createToken.Attached(this);
        }

        protected override void HandleDetach()
        {
            createToken = null;

            base.HandleDetach();
        }

        public void Accept(IUnitVisitor unitVisitor)
        {
            unitVisitor.Visit(this);
        }
    }
}