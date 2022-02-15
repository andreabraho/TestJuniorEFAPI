<template>
  <v-dialog max-width="290" :value="value" @input="v => $emit('input', v)">
  <v-card>
    <v-card-title class="headline">{{title}}</v-card-title>
    <v-card-text>{{bodyMessage}}</v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="green darken-1" text @click="$emit('modalResponse', false)">Disagree</v-btn>
      <v-btn color="green darken-1" text @click="$emit('modalResponse', true)">Agree</v-btn>
    </v-card-actions>
  </v-card>
</v-dialog>

</template>

<script>
export default {
  props: {
    value:null,
    title: {
      type: String,
      default: "Title",
      required: true,
    },
    bodyMessage: {
      type: String,
      default: "Body Message",
      required: true,
    },
  },
  vuetify: new Vuetify(),
  data: {
    showDialog: false,
  }
};
</script>

<style>
</style>