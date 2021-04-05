namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    public class CedenteDAO
    {
        #region Variables
        DataSet _dts = new DataSet();
        SoftCobEntities _dtb = new SoftCobEntities();
        List<CatalogosDTO> _catalogo = new List<CatalogosDTO>();
        string _mensaje = "";

        #endregion

        #region Procedimientos y Funciones Arbol
        public DataSet FunGetCiuadesCedentes()
        {
            try
            {
                var query = (from Cedente in _dtb.SoftCob_CEDENTE
                             join Ciudad in _dtb.SoftCob_CIUDAD on Cedente.cede_ciudcod equals Ciudad.CIUD_CODIGO
                             where Cedente.cede_estado == true
                             select new CatalogosDTO
                             {
                                 Descripcion = Ciudad.ciud_nombre,
                                 Codigo = Ciudad.CIUD_CODIGO.ToString()
                             }).Distinct();

                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public DataSet FunGetCedentesporIDCiudad(int _idciudad)
        {
            try
            {
                var query = from Cedente in _dtb.SoftCob_CEDENTE
                            where Cedente.cede_ciudcod.Equals(_idciudad) && Cedente.cede_estado == true
                            select new CatalogosDTO
                            {
                                Descripcion = Cedente.cede_nombre,
                                Codigo = Cedente.CEDE_CODIGO.ToString()
                            };
                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetProductosporIDCedente(int _idcedente)
        {
            try
            {
                var query = from ProductoCedente in _dtb.SoftCob_PRODUCTOS_CEDENTE
                            where ProductoCedente.CEDE_CODIGO.Equals(_idcedente) && ProductoCedente.prce_estado == true
                            select new CatalogosDTO
                            {
                                Descripcion = ProductoCedente.prce_producto,
                                Codigo = ProductoCedente.PRCE_CODIGO.ToString()
                            };
                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetCatalogoProductosporIDProducto(int _idproducto)
        {
            try
            {
                var query = from CatalogoProducto in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE
                            where CatalogoProducto.PRCE_CODIGO.Equals(_idproducto) && CatalogoProducto.cpce_estado == true
                            select new CatalogosDTO
                            {
                                Descripcion = CatalogoProducto.cpce_producto,
                                Codigo = CatalogoProducto.CPCE_CODIGO.ToString()
                            };
                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SoftCob_CEDENTE FunGetCedentePorID(int _codcedente)
        {
            SoftCob_CEDENTE _cedente = new SoftCob_CEDENTE();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _cedente = _db.SoftCob_CEDENTE.Where(e => e.CEDE_CODIGO == _codcedente).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _cedente;
        }
        public string FunGetNameCatalogoporID(int _codigocatalogo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _mensaje = _db.SoftCob_CATALOGO_PRODUCTOS_CEDENTE.Where(c => c.CPCE_CODIGO == _codigocatalogo).FirstOrDefault().cpce_producto;
                }
            }
            catch
            {
                _mensaje = "";
            }
            return _mensaje;
        }
        public DataSet FunGetCatalogoPorCiudad(int _codigociudad)
        {
            try
            {
                var query = from CTP in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE
                            join PRC in _dtb.SoftCob_PRODUCTOS_CEDENTE on CTP.PRCE_CODIGO equals PRC.PRCE_CODIGO
                            join CED in _dtb.SoftCob_CEDENTE on PRC.CEDE_CODIGO equals CED.CEDE_CODIGO
                            where CED.cede_ciudcod.Equals(_codigociudad) && CTP.cpce_estado == true && PRC.prce_estado == true
                            && CED.cede_estado == true
                            orderby CTP.cpce_producto
                            select new CatalogosDTO
                            {
                                Descripcion = CTP.cpce_producto,
                                Codigo = CTP.CPCE_CODIGO.ToString(),
                                Nivel = CED.cede_auxi1
                            };

                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetCedentes()
        {
            List<SoftCob_CEDENTE> _cedente = new List<SoftCob_CEDENTE>();

            _cedente = _dtb.SoftCob_CEDENTE.Where(pd => pd.cede_estado).OrderBy(o => o.cede_nombre).ToList();

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Cedente--",
                Codigo = "0"
            });

            foreach (SoftCob_CEDENTE _tab in _cedente)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _tab.cede_nombre,
                    Codigo = _tab.CEDE_CODIGO.ToString()
                });
            }

            _dts = new FuncionesDAO().FunCambiarDataSet(_catalogo);
            return _dts;
        }
        public DataSet FunGetCatalogoProducto(int _codigocedente)
        {
            try
            {
                var query = from Productos in _dtb.SoftCob_PRODUCTOS_CEDENTE
                            from Catalogo in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE
                            where Productos.CEDE_CODIGO.Equals(_codigocedente) && Catalogo.PRCE_CODIGO.Equals(Productos.PRCE_CODIGO)
                            orderby Catalogo.CPCE_CODIGO
                            select new CatalogoProductos
                            {
                                Producto = Productos.prce_producto,
                                CodigoCatalogo = Catalogo.CPCE_CODIGO.ToString(),
                                CodigoProducto = Catalogo.cpce_codigoproducto,
                                CatalogoProducto = Catalogo.cpce_producto,
                                CodigoFamilia = Catalogo.cpce_codigofamilia,
                                Familia = Catalogo.cpce_familia,
                                Estado = Catalogo.cpce_estado ? "Activo" : "Inactivo",
                                CodProducto = Catalogo.PRCE_CODIGO.ToString()
                            };

                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
