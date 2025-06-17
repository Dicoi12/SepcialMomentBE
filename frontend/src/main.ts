import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import PrimeVue from 'primevue/config'
import './style.css'
import App from './App.vue'

// PrimeVue styles
// import 'primevue/resources/themes/aura-light-blue/theme.css'
// import 'primevue/resources/primevue.css'
// import 'primeflex/primeflex.css'

// Create router instance
const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('./views/Home.vue')
    }
  ]
})

const app = createApp(App)

app.use(router)
app.use(PrimeVue)
app.mount('#app')
