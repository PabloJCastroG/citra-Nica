import React, { useState } from 'react';
import {
  StyleSheet,
  Text,
  View,
  ScrollView,
  Image,
  TouchableOpacity,
  Modal,
  Pressable,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import {
  Menu,
  Bell,
  MapPin,
  BadgeCheck,
  ShoppingBag,
  ListOrdered,
  Truck,
  CheckCircle2,
  Megaphone,
  CalendarDays,
  X,
  Home,
  ClipboardList,
  PlusCircle,
  MessageCircle,
  User,
  Settings,
  HelpCircle,
} from 'lucide-react-native';

export default function HomeScreen() {
  // Estado para controlar la visibilidad del menú lateral
  const [isMenuVisible, setIsMenuVisible] = useState(false);

  return (
    <SafeAreaView style={styles.safeArea}>
      <ScrollView
        contentContainerStyle={styles.container}
        showsVerticalScrollIndicator={false}
      >
        {/* HEADER */}
        <View style={styles.header}>
          {/* Al presionar las 3 rayas, abrimos el menú lateral */}
          <TouchableOpacity onPress={() => setIsMenuVisible(true)}>
            <Menu color="#1A2E2B" size={28} strokeWidth={2.5} />
          </TouchableOpacity>

          <View style={styles.logoContainer}>
            <Text style={styles.logoText}>
              <Text style={styles.logoGreen}>Citra</Text>
              <Text style={styles.logoOrange}>Nica</Text>
            </Text>
            <Text style={styles.logoSub}>de la finca a tu mesa 🍃</Text>
          </View>

          <TouchableOpacity>
            <Bell color="#1A2E2B" size={24} strokeWidth={2.5} />
          </TouchableOpacity>
        </View>

        {/* PROFILE CARD CENTRAL */}
        <View style={styles.profileCard}>
          <Image
            source={{
              uri: 'https://images.unsplash.com/photo-1595841696677-6489ff3f8cd1?auto=format&fit=crop&q=80&w=150&h=150',
            }}
            style={styles.profileImage}
          />
          <View style={styles.profileInfo}>
            <Text style={styles.greeting}>¡Bienvenido, Productor!</Text>
            <Text style={styles.farmName}>Finca el Paraiso 🍃</Text>
            <View style={styles.locationRow}>
              <MapPin color="#718096" size={14} />
              <Text style={styles.locationText}>Estelí, Condega</Text>
            </View>
          </View>
          <View style={styles.badge}>
            <Text style={styles.badgeText}>Productor</Text>
            <Text style={styles.badgeText}>Verificado</Text>
            <BadgeCheck color="#15803d" size={24} style={styles.badgeIcon} />
          </View>
        </View>

        {/* === RESTO DE CONTENIDO (Resumen, Productos, Avisos) === */}
        <Text style={styles.sectionTitle}>Resumen de Actividad</Text>
        <ScrollView
          horizontal
          showsHorizontalScrollIndicator={false}
          contentContainerStyle={styles.activityScroll}
        >
          <View style={styles.activityCard}>
            <ShoppingBag color="#15803d" size={28} />
            <Text style={styles.activityLabel}>Ventas Totales</Text>
            <Text style={[styles.activityValue, { color: '#15803d' }]}>
              C$ 205.00
            </Text>
            <Text style={styles.activitySub}>Este mes</Text>
          </View>
          <View style={styles.activityCard}>
            <ListOrdered color="#ea580c" size={28} />
            <Text style={styles.activityLabel}>Pedidos pendientes</Text>
            <Text style={[styles.activityValue, { color: '#ea580c' }]}>3</Text>
            <Text style={styles.activitySub}>Ver pedidos</Text>
          </View>
          <View style={styles.activityCard}>
            <CheckCircle2 color="#15803d" size={28} />
            <Text style={styles.activityLabel}>Pedidos completados</Text>
            <Text style={[styles.activityValue, { color: '#15803d' }]}>0</Text>
            <Text style={styles.activitySub}>Este mes</Text>
          </View>
        </ScrollView>

        <Text style={styles.sectionTitle}>Mis productos activos</Text>
        <ScrollView
          horizontal
          showsHorizontalScrollIndicator={false}
          contentContainerStyle={styles.productsScroll}
        >
          <View style={styles.productCard}>
            <View style={styles.productImageContainer}>
              <Image
                source={{
                  uri: 'https://images.unsplash.com/photo-1611080626919-7cf5a9dbab5b?auto=format&fit=crop&q=80&w=200&h=200',
                }}
                style={styles.productImage}
              />
            </View>
            <View style={styles.productDetails}>
              <Text style={styles.productName}>Naranja</Text>
              <Text style={styles.productPrice}>
                C$ 25.00 <Text style={styles.productUnit}>/ kg</Text>
              </Text>
            </View>
          </View>
          <View style={styles.productCard}>
            <View style={styles.productImageContainer}>
              <Image
                source={{
                  uri: 'https://images.unsplash.com/photo-1553279768-865429fa0078?auto=format&fit=crop&q=80&w=200&h=200',
                }}
                style={styles.productImage}
              />
            </View>
            <View style={styles.productDetails}>
              <Text style={styles.productName}>Mango</Text>
              <Text style={styles.productPrice}>
                C$ 20.00 <Text style={styles.productUnit}>/ kg</Text>
              </Text>
            </View>
          </View>
        </ScrollView>
      </ScrollView>

      {/* ========================================= */}
      {/* MENÚ LATERAL (SIDEBAR MODAL)              */}
      {/* ========================================= */}
      <Modal
        visible={isMenuVisible}
        transparent={true}
        animationType="fade" // Efecto de aparición suave
        onRequestClose={() => setIsMenuVisible(false)} // Comportamiento al presionar "Atrás" en Android
      >
        <View style={styles.modalOverlay}>
          {/* Fondo oscuro transparente, al presionarlo se cierra el menú */}
          <Pressable
            style={styles.modalBackground}
            onPress={() => setIsMenuVisible(false)}
          />

          {/* Contenedor del Menú Blanco */}
          <View style={styles.sidebar}>
            <SafeAreaView style={styles.sidebarSafeArea}>
              {/* Botón de Cerrar (X) */}
              <TouchableOpacity
                style={styles.closeMenuButton}
                onPress={() => setIsMenuVisible(false)}
              >
                <X color="#1A2E2B" size={28} strokeWidth={2.5} />
              </TouchableOpacity>

              {/* Perfil en el Menú */}
              <View style={styles.sidebarProfile}>
                <Image
                  source={{
                    uri: 'https://images.unsplash.com/photo-1595841696677-6489ff3f8cd1?auto=format&fit=crop&q=80&w=150&h=150',
                  }}
                  style={styles.sidebarProfileImage}
                />
                <View style={styles.sidebarProfileInfo}>
                  <Text style={styles.sidebarName}>Richard Valenzuela</Text>
                  <Text style={styles.sidebarFarm}>Finca el Paraiso 🍃</Text>
                  <View style={styles.sidebarBadgeRow}>
                    <Text style={styles.sidebarBadgeText}>
                      Productor Verificado
                    </Text>
                    <BadgeCheck
                      color="#15803d"
                      size={14}
                      style={{ marginLeft: 4 }}
                    />
                  </View>
                </View>
              </View>

              {/* Opciones del Menú */}
              <ScrollView
                showsVerticalScrollIndicator={false}
                contentContainerStyle={styles.menuOptionsContainer}
              >
                {/* Opción Activa (Inicio) */}
                <TouchableOpacity
                  style={[styles.menuItem, styles.menuItemActive]}
                >
                  <Home color="#E87722" size={24} strokeWidth={2.5} />
                  <Text
                    style={[styles.menuItemText, styles.menuItemTextActive]}
                  >
                    Inicio
                  </Text>
                </TouchableOpacity>

                {/* Otras Opciones */}
                <TouchableOpacity style={styles.menuItem}>
                  <ClipboardList color="#718096" size={24} />
                  <Text style={styles.menuItemText}>Mis pedidos</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.menuItem}>
                  <PlusCircle color="#718096" size={24} />
                  <Text style={styles.menuItemText}>Publicar producto</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.menuItem}>
                  <Megaphone color="#718096" size={24} />
                  <Text style={styles.menuItemText}>
                    Avisos Institucionales
                  </Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.menuItem}>
                  <MessageCircle color="#718096" size={24} />
                  <Text style={styles.menuItemText}>Mensajes</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.menuItem}>
                  <User color="#718096" size={24} />
                  <Text style={styles.menuItemText}>Mi perfil</Text>
                </TouchableOpacity>

                {/* Línea Divisora */}
                <View style={styles.menuDivider} />

                {/* Opciones Inferiores */}
                <TouchableOpacity style={styles.menuItem}>
                  <Bell color="#718096" size={24} />
                  <Text style={styles.menuItemText}>Notificaciones</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.menuItem}>
                  <Settings color="#718096" size={24} />
                  <Text style={styles.menuItemText}>Configuración</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.menuItem}>
                  <HelpCircle color="#718096" size={24} />
                  <Text style={styles.menuItemText}>Ayuda y soporte</Text>
                </TouchableOpacity>
              </ScrollView>
            </SafeAreaView>
          </View>
        </View>
      </Modal>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safeArea: { flex: 1, backgroundColor: '#FFFFFF' },
  container: { paddingBottom: 40 },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingHorizontal: 20,
    paddingVertical: 15,
  },
  logoContainer: { alignItems: 'center' },
  logoText: { fontSize: 22, fontWeight: '800' },
  logoGreen: { color: '#15803d' },
  logoOrange: { color: '#E87722' },
  logoSub: { fontSize: 10, color: '#718096', marginTop: 2 },

  // Perfil Principal
  profileCard: {
    flexDirection: 'row',
    backgroundColor: '#F0FDF4',
    borderWidth: 1,
    borderColor: '#bbf7d0',
    borderRadius: 20,
    padding: 16,
    marginHorizontal: 20,
    alignItems: 'center',
    marginBottom: 24,
  },
  profileImage: { width: 60, height: 60, borderRadius: 30, marginRight: 12 },
  profileInfo: { flex: 1 },
  greeting: { fontSize: 16, fontWeight: '700', color: '#1A2E2B' },
  farmName: { fontSize: 13, color: '#4A5568', marginTop: 2 },
  locationRow: { flexDirection: 'row', alignItems: 'center', marginTop: 4 },
  locationText: { fontSize: 12, color: '#718096', marginLeft: 4 },
  badge: {
    borderWidth: 1,
    borderColor: '#15803d',
    borderRadius: 12,
    paddingHorizontal: 8,
    paddingVertical: 6,
    alignItems: 'center',
    backgroundColor: '#FFFFFF',
    position: 'relative',
  },
  badgeText: { fontSize: 9, fontWeight: 'bold', color: '#15803d' },
  badgeIcon: {
    position: 'absolute',
    right: -10,
    top: -10,
    backgroundColor: '#FFF',
    borderRadius: 12,
  },

  // Listas horizontales
  sectionTitle: {
    fontSize: 16,
    fontWeight: '700',
    color: '#1A2E2B',
    marginLeft: 20,
    marginBottom: 12,
  },
  activityScroll: { paddingHorizontal: 16, paddingBottom: 24 },
  activityCard: {
    backgroundColor: '#FFFFFF',
    borderRadius: 16,
    padding: 16,
    width: 120,
    height: 140,
    marginHorizontal: 4,
    alignItems: 'center',
    justifyContent: 'center',
    elevation: 3,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.08,
    shadowRadius: 8,
  },
  activityLabel: {
    fontSize: 11,
    fontWeight: '600',
    color: '#1A2E2B',
    textAlign: 'center',
    marginTop: 8,
    height: 30,
  },
  activityValue: { fontSize: 16, fontWeight: 'bold', marginTop: 4 },
  activitySub: { fontSize: 10, color: '#A0AEC0', marginTop: 4 },
  productsScroll: { paddingHorizontal: 16, paddingBottom: 24 },
  productCard: {
    width: 140,
    marginHorizontal: 4,
    borderRadius: 16,
    backgroundColor: '#F7FAFC',
    overflow: 'hidden',
    borderWidth: 1,
    borderColor: '#EDF2F7',
  },
  productImageContainer: {
    backgroundColor: '#FFFFFF',
    alignItems: 'center',
    padding: 10,
  },
  productImage: { width: 100, height: 100, resizeMode: 'contain' },
  productDetails: { padding: 12, backgroundColor: '#E2E8F0' },
  productName: {
    fontSize: 14,
    fontWeight: '700',
    color: '#1A2E2B',
    marginBottom: 4,
  },
  productPrice: { fontSize: 13, fontWeight: '700', color: '#15803d' },
  productUnit: { fontSize: 11, color: '#718096', fontWeight: 'normal' },

  // =========================================
  // ESTILOS DEL MENÚ LATERAL MODAL
  // =========================================
  modalOverlay: {
    flex: 1,
    flexDirection: 'row',
  },
  modalBackground: {
    ...StyleSheet.absoluteFillObject,
    backgroundColor: 'rgba(0, 0, 0, 0.4)', // Fondo oscuro para resaltar el menú
  },
  sidebar: {
    width: '80%', // Ocupa el 80% de la pantalla
    height: '100%',
    backgroundColor: '#FFFFFF',
    borderTopRightRadius: 30,
    borderBottomRightRadius: 30,
    shadowColor: '#000',
    shadowOffset: { width: 4, height: 0 },
    shadowOpacity: 0.1,
    shadowRadius: 15,
    elevation: 10,
  },
  sidebarSafeArea: {
    flex: 1,
  },
  closeMenuButton: {
    padding: 20,
    alignSelf: 'flex-start',
  },
  sidebarProfile: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 20,
    marginBottom: 24,
  },
  sidebarProfileImage: {
    width: 55,
    height: 55,
    borderRadius: 27.5,
    marginRight: 12,
  },
  sidebarProfileInfo: {
    flex: 1,
  },
  sidebarName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#1A2E2B',
  },
  sidebarFarm: {
    fontSize: 13,
    color: '#4A5568',
    marginTop: 2,
  },
  sidebarBadgeRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 2,
  },
  sidebarBadgeText: {
    fontSize: 11,
    color: '#15803d',
    fontWeight: '600',
  },
  menuOptionsContainer: {
    paddingHorizontal: 20,
    paddingBottom: 40,
  },
  menuItem: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 14,
    paddingHorizontal: 16,
    borderRadius: 12,
    marginBottom: 8,
  },
  menuItemActive: {
    backgroundColor: '#F0FDF4', // Verde clarito para el activo
  },
  menuItemText: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1A2E2B',
    marginLeft: 16,
  },
  menuItemTextActive: {
    color: '#E87722', // Naranja para el texto activo
  },
  menuDivider: {
    height: 1,
    backgroundColor: '#EDF2F7',
    marginVertical: 16,
    marginHorizontal: 16,
  },
});
