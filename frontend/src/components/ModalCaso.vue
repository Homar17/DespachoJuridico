<template>
    <div class="fixed inset-0 bg-black/60 backdrop-blur-sm flex items-center justify-center p-4 z-50 transition-opacity duration-300">
      <div class="bg-despacho-oscuro text-despacho-fondo w-full max-w-lg rounded-3xl shadow-2xl overflow-hidden border border-despacho-fondo/10">
        
        <div class="p-6 pb-0 flex justify-between items-start">
          <div>
            <h3 class="text-2xl font-bold tracking-tight">Nuevo Expediente</h3>
            <div class="h-1 w-16 bg-despacho-rojo rounded-full mt-1.5 shadow-[0_0_10px_rgba(128,0,0,0.5)]"></div>
          </div>
          <button @click="$emit('cerrar')" class="text-despacho-fondo/50 hover:text-white transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-6 h-6">
              <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
  
        <form @submit.prevent="enviarFormulario" class="p-8 space-y-5">
          
          <div>
            <label class="block text-xs font-bold uppercase tracking-wider text-despacho-tierra mb-1.5">Título del Caso</label>
            <input 
              v-model="datos.titulo" 
              type="text" 
              placeholder="Ej. Divorcio Incausado - Pérez"
              class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all placeholder:text-despacho-fondo/20"
              required 
            />
          </div>
  
          <div>
            <label class="block text-xs font-bold uppercase tracking-wider text-despacho-tierra mb-1.5">Descripción</label>
            <textarea 
              v-model="datos.descripcion" 
              rows="3" 
              placeholder="Detalles preliminares del caso..."
              class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all placeholder:text-despacho-fondo/20"
            ></textarea>
          </div>
  
          <div class="grid grid-cols-2 gap-5">
            <div>
              <label class="block text-xs font-bold uppercase tracking-wider text-despacho-tierra mb-1.5">Rama Jurídica</label>
              <div class="relative">
                <select 
                  v-model.number="datos.id_RamaJuridica" 
                  class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all appearance-none cursor-pointer"
                  required 
                >
                  <option value="" disabled class="bg-despacho-oscuro text-despacho-fondo/50">Selecciona...</option>
                  <option 
                    v-for="rama in ramas" 
                    :key="rama.id" 
                    :value="rama.id"
                    class="bg-despacho-oscuro text-white"
                  >
                    {{ rama.descripcion }}
                  </option>
                </select>
                <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-4 text-despacho-fondo/50">
                  <svg class="h-4 w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path></svg>
                </div>
              </div>
            </div>
  
            <div>
              <label class="block text-xs font-bold uppercase tracking-wider text-despacho-tierra mb-1.5">Estatus Inicial</label>
              <div class="relative">
                <select 
                  v-model.number="datos.id_estatus" 
                  class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all appearance-none cursor-pointer"
                  required 
                >
                  <option value="" disabled class="bg-despacho-oscuro text-despacho-fondo/50">Selecciona...</option>
                  <option 
                    v-for="estatus in listaEstatus" 
                    :key="estatus.id" 
                    :value="estatus.id"
                    class="bg-despacho-oscuro text-white"
                  >
                    {{ estatus.descripcion }}
                  </option>
                </select>
                <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-4 text-despacho-fondo/50">
                  <svg class="h-4 w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path></svg>
                </div>
              </div>
            </div>
          </div>
  
          <div class="pt-6 flex gap-4">
            <button 
              type="button" 
              @click="$emit('cerrar')" 
              class="flex-1 py-3 border border-despacho-fondo/30 text-despacho-fondo/70 font-semibold rounded-xl hover:bg-white/5 hover:text-white transition-all active:scale-95"
            >
              Cancelar
            </button>
            <button 
              type="submit" 
              class="flex-1 py-3 bg-despacho-rojo text-white font-bold rounded-xl shadow-lg hover:bg-despacho-rojo/90 transition-all active:scale-95 shadow-despacho-rojo/30"
            >
              Guardar Expediente
            </button>
          </div>
        </form>
      </div>
    </div>
  </template>
  
  <script setup>
  import { reactive, ref, onMounted } from 'vue'
  import axios from 'axios'
  
  const props = defineProps({
    clienteId: { type: Number, default: 1 },
    tituloDefecto: { type: String, default: '' },
    descripcionDefecto: { type: String, default: '' }
  })
  
  const emit = defineEmits(['cerrar', 'creado'])
  
  const ramas = ref([])
  const listaEstatus = ref([])
  
  const datos = reactive({
    titulo: props.tituloDefecto,
    descripcion: props.descripcionDefecto,
    id_usuario: props.clienteId, 
    id_estatus: '',
    id_RamaJuridica: '' 
  })
  
  const cargarCatalogos = async () => {
    const token = localStorage.getItem('token')
    if (!token) return
  
    try {
      const [resRamas, resEstatus] = await Promise.all([
        axios.get('http://localhost:5039/api/RamasJuridicas', { headers: { Authorization: `Bearer ${token}` } }),
        axios.get('http://localhost:5039/api/Estatus', { headers: { Authorization: `Bearer ${token}` } })
      ])
  
      ramas.value = resRamas.data
      listaEstatus.value = resEstatus.data
  
      if (ramas.value.length > 0) datos.id_RamaJuridica = ramas.value[0].id
      if (listaEstatus.value.length > 0) datos.id_estatus = listaEstatus.value[0].id
  
    } catch (error) {
      console.error("Error al cargar Ramas o Estatus desde el backend:", error)
    }
  }
  
  onMounted(() => {
    cargarCatalogos()
  })
  
  const enviarFormulario = () => {
    emit('creado', { ...datos })
    datos.titulo = ''
    datos.descripcion = ''
  }
  </script>