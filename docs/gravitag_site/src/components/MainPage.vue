<template>
  <v-container>
    <section>
      <v-img src="../assets/Gravitag.png"></v-img>
    </section>
    <section>
      <iframe style="float:center" width=550 height=300 align="middle" src="https://www.youtube.com/embed/m_gX31bQTto" frameborder="1" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
    </section>
    <v-btn v-on:click="spawn('about')" text icon color="blue">
      <v-icon>
        assignment
      </v-icon>
    </v-btn>
    <v-card v-if="about_isHidden">
      <About/>
    </v-card>
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
</style>
