<template>
<div >
  <v-app>
    <v-content class="bg">
      <HelloWorld/>
    </v-content>
  </v-app>
</div>
</template>

<script>
import HelloWorld from './components/HelloWorld';


export default {
  name: 'App',
  components: {
    HelloWorld,
  },
  props: {
    source: String
  }
};
</script>

<style>
  .bg {
    /* The image used */
    background-image: url("./assets/Outer-Space-Wallpaper.jpg");

    /* Full height */
    height: 100%;

    /* Center and scale the image nicely */
    background-position: center;
    background-repeat: no-repeat;
    background-size: cover;
  }
</style>
