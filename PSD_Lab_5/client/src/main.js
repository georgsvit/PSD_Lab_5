import Vue from 'vue'
import App from './App.vue'
import router from './router'
import VueResource from 'vue-resource'
import VueBootstrap from 'bootstrap-vue'

import '@/assets/main.css'

Vue.config.productionTip = false

Vue.use(VueResource)
Vue.use(VueBootstrap)

Vue.http.options.root = 'https://192.168.178.21:5001/'

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
