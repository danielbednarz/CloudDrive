<template>
  <q-item
    clickable
    tag="a"
    :target="isTargetBlank ? `_blank` : ``"
    :href="getLink()"
  >
    <q-item-section v-if="icon" avatar>
      <q-icon :name="icon" />
    </q-item-section>

    <q-item-section>
      <q-item-label>{{ title }}</q-item-label>
      <q-item-label caption>{{ caption }}</q-item-label>
    </q-item-section>
  </q-item>
  <q-separator inset class="q-my-md" v-if="separator" />
</template>

<script>
import { defineComponent } from "vue";

export default defineComponent({
  name: "EssentialLink",
  props: {
    title: {
      type: String,
      required: true,
    },

    caption: {
      type: String,
      default: "",
    },

    link: {
      type: String,
      default: "#",
    },

    internalPathName: {
      type: String,
      default: "",
    },

    icon: {
      type: String,
      default: "",
    },

    isTargetBlank: {
      type: Boolean,
      default: false,
    },

    separator: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    getLink() {
      if (this.internalPathName) {
        let props = this.$router.resolve({
          name: this.internalPathName,
        });
        return props.href;
      } else {
        return this.link;
      }
    },
  },
});
</script>
