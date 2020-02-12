<template>
  <v-container>
    <section>
      <v-img class="resp-image" src="../assets/Gravitag.png"></v-img>
    </section>
    <div class="resp-container">
      <iframe class="resp-iframe" align="center" src="https://www.youtube.com/embed/m_gX31bQTto" frameborder="1" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
    </div>
    <v-container>
      <v-btn v-on:click="spawn('about')" text large icon color="white">
      <v-icon>
        assignment
      </v-icon>
    </v-btn>
    <v-card v-if="about_isHidden">
      <About/>
    </v-card>
    </v-container>
  </v-container>
</template>

<script>
import About from './About'
export default {
  name: 'MainPage',
  components: {
    About,
  },
  data: function () {
    return {
      about_isHidden: false
    } 
  },
  methods: {
    spawn: function (msg){
      if (msg == 'about' && this.about_isHidden == true) {
        return this.about_isHidden = false
      } else if (msg == 'about' && this.about_isHidden == false) {
        return this.about_isHidden = true
      }
    }
  },
};

</script>

<style scoped>
  .headline {
    color: white
  }
  .white--text{
    color: white
  }
  .resp-container {
    position: relative;
    padding-top: 56.25%;
  }
  .resp-iframe {
    position: absolute;
    top: 0;
    left: 25%;
    width: 50%;
    height: 50%;
    border: 1;
  }
  .resp-image{
    max-width: 75%;
    height: auto;
    left: 12%;
  }
</style>
