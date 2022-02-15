import Vue from 'vue'
import App from './App.vue'
import router from '../router'

import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
//https://www.npmjs.com/package/vue2-toast//
import 'vue2-toast/lib/toast.css';
import Toast from 'vue2-toast';
// Import Bootstrap an BootstrapVue CSS files (order is important)
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
// Make BootstrapVue available throughout your project
import vueDebounce from 'vue-debounce'

Vue.use(vueDebounce)
Vue.use(BootstrapVue)
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin)
Vue.use(Toast);

Vue.config.productionTip = false

new Vue({
  render: h => h(App),
  router
}).$mount('#app')
