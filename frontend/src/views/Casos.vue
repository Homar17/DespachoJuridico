<template>
    <div class="min-h-screen bg-despacho-oscuro p-6 md:p-10 text-despacho-fondo relative">
      <div class="max-w-7xl mx-auto">
        
        <header class="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-6 pb-6 border-b border-despacho-fondo/10">
          <div>
            <h2 class="text-3xl font-bold tracking-tight text-despacho-fondo">Expedientes</h2>
            <div class="h-1 w-16 bg-despacho-rojo rounded-full mt-1.5 shadow-[0_0_10px_rgba(128,0,0,0.5)]"></div>
            <p class="text-despacho-fondo/60 font-medium mt-1">Gestión administrativa y jurídica</p>
          </div>
          
          <button 
            @click="cerrarSesion" 
            class="flex items-center justify-center gap-2 px-5 py-2.5 bg-despacho-rojo text-white font-semibold rounded-xl hover:bg-despacho-rojo/90 transition-all active:scale-95 shadow-lg shadow-black/20"
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-5 h-5">
              <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 9V5.25A2.25 2.25 0 0013.5 3h-6a2.25 2.25 0 00-2.25 2.25v13.5A2.25 2.25 0 007.5 21h6a2.25 2.25 0 002.25-2.25V15m3 0l3-3m0 0l-3-3m3 3H9" />
            </svg>
            Cerrar Sesión
          </button>
        </header>
  
        <div class="mb-10 relative max-w-2xl">
          <span class="absolute inset-y-0 left-0 flex items-center pl-4 text-despacho-fondo/30">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-5 h-5">
              <path stroke-linecap="round" stroke-linejoin="round" d="M21 21l-5.197-5.197m0 0A7.5 7.5 0 105.196 5.196a7.5 7.5 0 0010.607 10.607z" />
            </svg>
          </span>
          <input 
            v-model="textoBusqueda"
            type="text" 
            placeholder="Buscar expediente, cliente o rama..." 
            class="w-full pl-12 pr-4 py-3 bg-white/5 border border-despacho-fondo/10 rounded-2xl focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all shadow-inner text-despacho-fondo placeholder:text-despacho-fondo/20"
          />
        </div>
  
        <main>
          <div v-if="casos.length === 0" class="flex flex-col items-center justify-center py-20 text-despacho-fondo/20">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-despacho-rojo mb-4"></div>
            <p class="font-medium">Sincronizando con el servidor...</p>
          </div>
  
          <div v-else-if="casosFiltrados.length === 0" class="text-center py-20 text-despacho-fondo/40 font-medium text-xl italic">
            No hay coincidencias para "{{ textoBusqueda }}"
          </div>
          
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div 
              v-for="caso in casosFiltrados" 
              :key="caso.id" 
              class="bg-white/5 backdrop-blur-sm rounded-3xl shadow-xl border border-white/5 hover:border-despacho-rojo/30 hover:bg-white/10 transition-all relative overflow-hidden group p-7"
            >
              <div class="absolute left-0 top-0 bottom-0 w-1 bg-despacho-rojo/30 group-hover:bg-despacho-rojo transition-colors"></div>
  
              <div class="flex justify-between items-start mb-6 gap-3">
                <h3 class="text-xl font-bold leading-tight text-despacho-fondo">{{ caso.titulo }}</h3>
                <span class="bg-despacho-rojo/20 text-despacho-rosa text-[10px] font-black px-3 py-1 rounded-full uppercase tracking-tighter border border-despacho-rosa">
                  {{ caso.estatus?.descripcion || 'Pendiente' }}
                </span>
              </div>
  
              <div class="space-y-4 text-sm">
                <div class="flex items-center gap-3">
                  <span class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra w-16">Cliente</span>
                  <span class="text-despacho-fondo font-medium">{{ caso.usuario?.nombre }}</span>
                </div>
                
                <div class="flex items-center gap-3">
                  <span class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra w-16">Rama</span>
                  <span class="text-despacho-fondo/80">{{ caso.ramaJuridica?.descripcion }}</span>
                </div>
              </div>
  
              <div class="mt-8 pt-5 border-t border-white/5 flex justify-end">
                <button class="text-despacho-tierra font-bold text-xs flex items-center gap-2 hover:text-despacho-rosa transition-colors group/btn">
                  DETALLES DEL CASO
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" class="w-4 h-4 transform group-hover/btn:translate-x-1 transition-transform">
                    <path fill-rule="evenodd" d="M3 10a.75.75 0 01.75-.75h10.638L10.23 5.29a.75.75 0 111.04-1.08l5.5 5.25a.75.75 0 010 1.08l-5.5 5.25a.75.75 0 11-1.04-1.08l4.158-3.96H3.75A.75.75 0 013 10z" clip-rule="evenodd" />
                  </svg>
                </button>
              </div>
            </div>
          </div>
        </main>
  
        <button 
          @click="mostrarModal = true"
          class="fixed bottom-10 right-10 w-16 h-16 bg-despacho-rojo text-white rounded-full shadow-[0_10px_25px_rgba(128,0,0,0.4)] flex items-center justify-center hover:scale-110 hover:rotate-90 transition-all active:scale-95 z-40"
        >
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="3" stroke="currentColor" class="w-8 h-8">
            <path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
          </svg>
        </button>
  
        <ModalCaso 
          v-if="mostrarModal" 
          @cerrar="mostrarModal = false" 
          @creado="guardarCasoEnBackend"
        />
  
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted, computed } from 'vue'
  import axios from 'axios'
  import { useRouter } from 'vue-router'
  import ModalCaso from '../components/ModalCaso.vue'
  
  const casos = ref([])
  const textoBusqueda = ref('')
  const mostrarModal = ref(false)
  const router = useRouter()
  
  const casosFiltrados = computed(() => {
    if (!textoBusqueda.value) return casos.value
    const busqueda = textoBusqueda.value.toLowerCase()
    return casos.value.filter(caso => {
      return (
        caso.titulo.toLowerCase().includes(busqueda) ||
        caso.usuario?.nombre?.toLowerCase().includes(busqueda) ||
        caso.ramaJuridica?.descripcion?.toLowerCase().includes(busqueda)
      )
    })
  })
  
  const obtenerCasos = async () => {
    const token = localStorage.getItem('token')
    if (!token) { router.push('/login'); return }
    try {
      const respuesta = await axios.get('http://localhost:5039/api/casos', {
        headers: { Authorization: `Bearer ${token}` }
      })
      casos.value = respuesta.data
    } catch (error) {
      cerrarSesion()
    }
  }
  
  const guardarCasoEnBackend = async (datosNuevos) => {
    const token = localStorage.getItem('token')
    try {
      await axios.post('http://localhost:5039/api/casos', datosNuevos, {
        headers: { Authorization: `Bearer ${token}` }
      })
      mostrarModal.value = false
      obtenerCasos()
    } catch (e) {
      alert("Error al guardar: Verifica los IDs")
    }
  }
  
  const cerrarSesion = () => {
    localStorage.removeItem('token')
    router.push('/login')
  }
  
  onMounted(obtenerCasos)
  </script>