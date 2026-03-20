<template>
    <div class="min-h-screen bg-despacho-oscuro p-6 md:p-10 text-despacho-fondo relative">
      <div class="max-w-7xl mx-auto">
        
        <header class="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-10 pb-6 border-b border-despacho-fondo/10">
          <div>
            <h2 class="text-3xl font-bold tracking-tight text-despacho-fondo">Agenda de Asesorías</h2>
            <div class="h-1 w-16 bg-despacho-rojo rounded-full mt-1.5 shadow-[0_0_10px_rgba(128,0,0,0.5)]"></div>
            <p class="text-despacho-fondo/60 font-medium mt-1">Gestión de citas programadas</p>
          </div>
          
          <div class="flex items-center gap-3">
            <router-link 
              to="/" 
              class="text-xs font-bold uppercase tracking-widest text-despacho-tierra hover:text-white transition-colors"
            >
              Ir al Inicio
            </router-link>
            <router-link 
              to="/casos" 
              class="px-5 py-2.5 bg-white/5 border border-white/10 text-despacho-fondo font-semibold rounded-xl hover:bg-white/10 transition-all active:scale-95"
            >
              Ver Expedientes
            </router-link>
            
            <button 
              @click="cerrarSesion" 
              class="flex items-center justify-center gap-2 px-5 py-2.5 bg-despacho-rojo text-white font-semibold rounded-xl hover:bg-despacho-rojo/90 transition-all active:scale-95 shadow-lg shadow-black/20 text-sm"
            >
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-5 h-5">
                <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 9V5.25A2.25 2.25 0 0013.5 3h-6a2.25 2.25 0 00-2.25 2.25v13.5A2.25 2.25 0 007.5 21h6a2.25 2.25 0 002.25-2.25V15m3 0l3-3m0 0l-3-3m3 3H9" />
              </svg>
              Cerrar Sesión
            </button>
          </div>
        </header>
  
        <main>
          <div v-if="cargando" class="flex flex-col items-center justify-center py-20 text-despacho-fondo/20">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-despacho-rojo mb-4"></div>
            <p class="font-medium">Cargando la agenda del despacho...</p>
          </div>
  
          <div v-else-if="citas.length === 0" class="text-center py-20 text-despacho-fondo/40 font-medium text-xl italic">
            No hay citas programadas por el momento.
          </div>
          
          <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div 
              v-for="cita in citas" 
              :key="cita.id" 
              class="bg-white/5 backdrop-blur-sm rounded-3xl shadow-xl border border-white/5 hover:border-despacho-rojo/30 hover:bg-white/10 transition-all relative overflow-hidden group p-7 flex flex-col"
            >
              <div class="absolute left-0 top-0 bottom-0 w-1 bg-despacho-rojo/30 group-hover:bg-despacho-rojo transition-colors"></div>
  
              <div class="mb-5 border-b border-white/10 pb-4">
                <h3 class="text-xl font-black text-despacho-rojo capitalize leading-tight mb-1">
                  {{ formatearFecha(cita.fechaHora) }}
                </h3>
                <div class="flex items-center gap-2 text-despacho-fondo">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4 text-despacho-tierra">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M12 6v6h4.5m4.5 0a9 9 0 11-18 0 9 9 0 0118 0z" />
                  </svg>
                  <span class="font-bold tracking-widest text-sm">{{ formatearHora(cita.fechaHora) }}</span>
                </div>
              </div>
  
              <div class="space-y-4 text-sm flex-1">
                <div>
                  <span class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra block mb-1">Cliente</span>
                  <span class="text-despacho-fondo font-medium text-base">{{ cita.usuario?.nombre || 'Desconocido' }}</span>
                </div>
                
                <div>
                  <span class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra block mb-1">Motivo de la Consulta</span>
                  <p class="text-despacho-fondo/80 leading-relaxed">{{ cita.descripcion }}</p>
                </div>
              </div>
  
              <div class="mt-6 pt-4 flex justify-end border-t border-white/5">
                <button 
                  @click="abrirModalExpediente(cita)"
                  class="text-xs font-bold uppercase tracking-widest text-despacho-tierra hover:text-white bg-despacho-rosa/20 hover:bg-despacho-rojo px-4 py-2 rounded-xl transition-all active:scale-95 flex items-center gap-2"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-4 h-4">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
                  </svg>
                  Iniciar Expediente
                </button>
              </div>
            </div>
          </div>
        </main>
  
        <ModalCaso 
          v-if="mostrarModalCaso" 
          :clienteId="datosPrecargados.clienteId"
          :tituloDefecto="datosPrecargados.titulo"
          :descripcionDefecto="datosPrecargados.descripcion"
          @cerrar="mostrarModalCaso = false" 
          @creado="procesarNuevoCaso"
        />

      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import axios from 'axios'
  import { useRouter } from 'vue-router'
  import ModalCaso from '../components/ModalCaso.vue'
  
  const citas = ref([])
  const cargando = ref(true)
  const router = useRouter()
  
  const mostrarModalCaso = ref(false)
  const datosPrecargados = ref({ clienteId: null, titulo: '', descripcion: '' })

  const formatearFecha = (fechaIso) => {
    const fecha = new Date(fechaIso)
    const opciones = { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' }
    return fecha.toLocaleDateString('es-MX', opciones)
  }
  
  const formatearHora = (fechaIso) => {
    const fecha = new Date(fechaIso)
    return fecha.toLocaleTimeString('es-MX', { hour: '2-digit', minute: '2-digit' }) + ' hrs'
  }
  
  const obtenerCitas = async () => {
    const token = localStorage.getItem('token')
    if (!token) { 
      router.push('/login')
      return 
    }
  
    try {
      const respuesta = await axios.get('http://localhost:5039/api/citas', {
        headers: { Authorization: `Bearer ${token}` }
      })
      
      citas.value = respuesta.data.sort((a, b) => new Date(a.fechaHora) - new Date(b.fechaHora))
    } catch (error) {
      console.error(error)
      if (error.response?.status === 401) {
        cerrarSesion()
      }
    } finally {
      cargando.value = false
    }
  }
  
  const abrirModalExpediente = (cita) => {
    datosPrecargados.value = {
      clienteId: cita.id_user,
      titulo: `Expediente - ${cita.usuario?.nombre || 'Cliente'}`,
      descripcion: `Cita: ${formatearFecha(cita.fechaHora)} a las ${formatearHora(cita.fechaHora)}.\nMotivo: ${cita.descripcion}`
    }
    mostrarModalCaso.value = true
  }

  const procesarNuevoCaso = async (datosNuevos) => {
    const token = localStorage.getItem('token')
    try {
      await axios.post('http://localhost:5039/api/casos', datosNuevos, {
        headers: { Authorization: `Bearer ${token}` }
      })
      mostrarModalCaso.value = false
      alert("¡Expediente creado exitosamente!")
      
      router.push('/casos')
    } catch (e) {
      console.error(e)
      alert("Error al crear el expediente. Verifica que la Rama y el Estatus existan en tu base de datos.")
    }
  }

  const cerrarSesion = () => {
    localStorage.removeItem('token')
    router.push('/login')
  }
  
  onMounted(obtenerCitas)
  </script>