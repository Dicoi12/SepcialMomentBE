import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config'
import './style.css'
import App from './App.vue'
import '/node_modules/primeflex/primeflex.css'
import 'primeflex/primeflex.css';
import 'primeicons/primeicons.css'
import Aura from '@primeuix/themes/aura';
import router from './router'

// PrimeVue styles
// import 'primevue/resources/themes/aura-light-blue/theme.css'
// import 'primevue/resources/primevue.css'
// import 'primeflex/primeflex.css'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)
app.use(PrimeVue, {
  theme: {
      preset: Aura,
      options: {
          prefix: 'p',
          cssLayer: false,
          darkModeSelector: false || 'none',
      }
  }
});
app.mount('#app')
