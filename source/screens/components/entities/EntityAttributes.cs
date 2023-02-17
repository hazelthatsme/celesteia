using System;
using System.Linq;

namespace Celestia.Screens.Components {
    public class EntityAttributes {
        public EntityAttributeMap Attributes;

        public EntityAttributes(EntityAttributeMap attributes) {
            Attributes = attributes;
        }

        public EntityAttributes() : this(new EntityAttributeMap()) {}

        public partial class EntityAttributeMap {
            private float[] attributes;

            public EntityAttributeMap() {
                attributes = new float[Enum.GetValues(typeof(EntityAttribute)).Cast<EntityAttribute>().Count()];
            }

            public EntityAttributeMap Set(EntityAttribute attribute, float value) {
                attributes[(int) attribute] = value;
                return this;
            }

            public float Get(EntityAttribute attribute) {
                return attributes[(int) attribute];
            }
        }
    }

    public enum EntityAttribute {
        MovementSpeed    
    }
}