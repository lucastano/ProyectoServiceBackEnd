
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;



namespace ProyectoService.LogicaNegocio.Modelo
{
	[Table("reparaciones")]
	public class Reparacion
	{

		public int Id { get; set; }
		public DateTime Fecha { get; set; } = DateTime.Now;

		public Tecnico Tecnico { get; set; }
		public Cliente Cliente { get; set; }
		public Producto Producto { get; set; }
		public string NumeroSerie { get; set; }
		public string Descripcion { get; set; }
		public DateTime FechaPromesaPresupuesto { get; set; }
		public string Estado { get; set; }
		public string DescripcionPresupuesto { get; set; }
		public string RazonNoAceptada { get; set; }
		public DateTime FechaPresupuesto { get; set; }
		public DateTime FechaPromesaEntrega { get; set; }
		public double ManoDeObra { get; set; }
		public double CostoFinal { get; set; }
		public DateTime FechaEntrega { get; set; }
		public bool Reparada { get; set; }
		public ICollection<Mensaje>Mensajes { get; set; }
		
		public Reparacion()
		{

			this.Estado = "EnTaller";
			this.DescripcionPresupuesto = string.Empty;
			this.ManoDeObra = 0;
			this.CostoFinal = 0;
			this.RazonNoAceptada = string.Empty;
			QuestPDF.Settings.License = LicenseType.Community;
		}


		public void Presupuestar(double costo, string descripcion, DateTime fechaPromesaEntrega)
		{
			this.ManoDeObra = costo;
			this.CostoFinal = ManoDeObra;
			this.Estado = "Presupuestada";
			this.DescripcionPresupuesto = descripcion;
			this.FechaPresupuesto = DateTime.Now;
			this.FechaPromesaEntrega = fechaPromesaEntrega;
		}

		public void AceptarPresupuesto()
		{
			this.Estado = "PresupuestoAceptado";
		}
		public void NoAceptarPresupuesto(double costo, string razon)
		{
			this.Estado = "PresupuestoNoAceptado";
			this.RazonNoAceptada = razon;
			this.CostoFinal = costo;
		}
		public void Terminar(bool reparada)
		{
			
			this.Estado = "Terminada";
			this.Reparada = reparada;

		}

		public void Entregar()
		{
			this.FechaEntrega = DateTime.Now;
			this.Estado = "Entregada";
		}
		public byte[] GenerarOrdenDeServicio(Empresa emp)
		{
			if (this.Estado == "EnTaller")
			{
				return GenerarPdfOrdenServicioEntrada(emp);
			}
			else if (this.Estado == "Entregada")
			{
                return GenerarPdfOrdenServicioEntregada(emp);

            }
            else
			{
                return GenerarPdfOrdenServicioPresupuestada(emp);

            }
        }

		public byte[] GenerarPdfOrdenServicioEntrada(Empresa emp)
		{
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", "Empresa.png");
			byte[] imageData=File.ReadAllBytes(imagePath);
            var data = Document.Create(document =>
{
		document.Page(page =>
		{
			page.Margin(30);

					page.Header().ShowOnce().Row(row =>
					{

			
						row.ConstantItem(150).Image(imageData);
						//row.ConstantItem(100).Height(150).Image(imageData, ImageScaling.FitArea);


						row.RelativeItem().Column(col =>
						{ // estos van a ser datos de la empresa. 
							col.Item().AlignCenter().Text(emp.Nombre.ToUpper()).Bold().FontSize(14);
							col.Item().AlignCenter().Text(emp.Direccion).FontSize(9);
							col.Item().AlignCenter().Text(emp.Telefono).FontSize(9);
							col.Item().AlignCenter().Text(emp.Email).FontSize(9);
						});

						row.RelativeItem().Column(col =>
						{
							col.Item().Border(1).BorderColor("#257272")
							.AlignCenter().Text("Numero de Orden");
							//ID
							col.Item().Background("#257272").Border(1)
							.BorderColor("#257272").AlignCenter()
							.Text(Id.ToString()).FontColor("#fff");
							//FECHA DE INGRESO
							col.Item().Border(1).BorderColor("#257272")
							.AlignCenter().Text(Fecha.ToString("dd-MM-yyyy"));
						});
					});

					page.Content().PaddingVertical(10).Column(col1 =>
					{ //DATOS DEL CLIENTE
						col1.Item().Column(col2 =>
						{
							col2.Item().Text("Datos del cliente").Underline().Bold();

							col2.Item().Text(txt =>
							{
								txt.Span("Nombre: ").SemiBold().FontSize(10);
								txt.Span(Cliente.Nombre + " " + Cliente.Apellido).FontSize(10);
							});

							col2.Item().Text(txt =>
							{
								txt.Span("Celular/Telefono: ").SemiBold().FontSize(10);
								txt.Span(Cliente.Telefono).FontSize(10);
							});

							col2.Item().Text(txt =>
							{
								txt.Span("Email: ").SemiBold().FontSize(10);
								txt.Span(Cliente.Email.Value).FontSize(10);
							});
							col2.Item().Text(txt =>
							{
								txt.Span("Direccion: ").SemiBold().FontSize(10);
								txt.Span(Cliente.Direccion).FontSize(10);
							});
						});

						col1.Item().LineHorizontal(0.5f);

						col1.Item().Table(tabla =>
						{
							tabla.ColumnsDefinition(columns =>
							{
								columns.RelativeColumn(3);
								columns.RelativeColumn(3);
								columns.RelativeColumn(5);
								columns.RelativeColumn(2);


							});

							tabla.Header(header =>
							{
								header.Cell().Background("#257272")
								.Padding(2).Text("Producto").FontColor("#fff");

								header.Cell().Background("#257272")
								.Padding(2).Text("Numero de serie").FontColor("#fff");

								header.Cell().Background("#257272")
								.Padding(2).Text("Descripcion problema").FontColor("#fff");
								header.Cell().Background("#257272")
							   .Padding(2).Text("Tecnico").FontColor("#fff");

							});




							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(Producto.Marca+" "+Producto.Modelo + " " +Producto.Version).FontSize(10);

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(NumeroSerie).FontSize(10);

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(Descripcion).FontSize(10);

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(Tecnico.Nombre).FontSize(10);



						});


			if (1 == 1)
				col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
				.Column(column =>
				{
					column.Item().Text("Politicas").FontSize(14);
					//aca va algun atributo de empresa con sus politicas
					column.Item().Text(emp.PoliticasEmpresa);
					column.Spacing(5);
				});

			col1.Spacing(10);
		});

		page.Footer()
		.AlignRight()
		.Text(txt =>
		{
			txt.Span("Pagina ").FontSize(10);
			txt.CurrentPageNumber().FontSize(10);
			txt.Span(" de ").FontSize(10);
			txt.TotalPages().FontSize(10);
		});
	});
}).GeneratePdf();

			return data;



		}
		public byte[] GenerarPdfOrdenServicioPresupuestada(Empresa emp)
		{
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", "Empresa.png");
            byte[] imageData = File.ReadAllBytes(imagePath);
            var data =Document.Create(document =>
						{
					document.Page(page =>
						{
						 page.Margin(30);

	 page.Header().ShowOnce().Row(row =>
	 {
		 

		 //row.ConstantItem(140).Height(60).Placeholder();
		 row.ConstantItem(150).Image(imageData);

		 row.RelativeItem().Column(col =>
		 { // estos van a ser datos de la empresa. 
			 col.Item().AlignCenter().Text(emp.Nombre).Bold().FontSize(14);
			 col.Item().AlignCenter().Text(emp.Direccion).FontSize(9);
			 col.Item().AlignCenter().Text(emp.Telefono).FontSize(9);
			 col.Item().AlignCenter().Text(emp.Email).FontSize(9);
		 });

		 row.RelativeItem().Column(col =>
		 {
			 col.Item().Border(1).BorderColor("#257272")
			 .AlignCenter().Text("Numero de Orden");
			 //ID
			 col.Item().Background("#257272").Border(1)
			 .BorderColor("#257272").AlignCenter()
			 .Text(Id.ToString()).FontColor("#fff");
			 //FECHA DE INGRESO
			 col.Item().Border(1).BorderColor("#257272")
			 .AlignCenter().Text(Fecha.ToString("dd-MM-yyyy"));
		 });
	 });

	 page.Content().PaddingVertical(10).Column(col1 =>
	 { //DATOS DEL CLIENTE
		 col1.Item().Column(col2 =>
		 {
			 col2.Item().Text("Datos del cliente").Underline().Bold();

			 col2.Item().Text(txt =>
			 {
				 txt.Span("Nombre: ").SemiBold().FontSize(10);
				 txt.Span(Cliente.Nombre + " " + Cliente.Apellido).FontSize(10);
			 });

			 col2.Item().Text(txt =>
			 {
				 txt.Span("Celular/Telefono: ").SemiBold().FontSize(10);
				 txt.Span(Cliente.Telefono).FontSize(10);
			 });

			 col2.Item().Text(txt =>
			 {
				 txt.Span("Email: ").SemiBold().FontSize(10);
				 txt.Span(Cliente.Email.Value).FontSize(10);
			 });
			 col2.Item().Text(txt =>
			 {
				 txt.Span("Direccion: ").SemiBold().FontSize(10);
				 txt.Span(Cliente.Direccion).FontSize(10);
			 });
		 });

		 col1.Item().LineHorizontal(0.5f);

		 col1.Item().Table(tabla =>
		 {
			 tabla.ColumnsDefinition(columns =>
			 {
                 columns.RelativeColumn(3);
                 columns.RelativeColumn(3);
                 columns.RelativeColumn(5);
                 columns.RelativeColumn(2);


             });

			 tabla.Header(header =>
			 {
				 header.Cell().Background("#257272")
				 .Padding(2).Text("Producto").FontColor("#fff");

				 header.Cell().Background("#257272")
				 .Padding(2).Text("Numero de serie").FontColor("#fff");

				 header.Cell().Background("#257272")
				 .Padding(2).Text("Descripcion problema").FontColor("#fff");
				 header.Cell().Background("#257272")
				.Padding(2).Text("Tecnico").FontColor("#fff");

			 });




			 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
             .Padding(2).Text(Producto.Marca + " " + Producto.Modelo + " " + Producto.Version).FontSize(10);

             tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
			 .Padding(2).Text(NumeroSerie).FontSize(10);

			 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
			 .Padding(2).Text(Descripcion).FontSize(10);

			 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
			 .Padding(2).Text(Tecnico.Nombre).FontSize(10);



		 });
		 

		 col1.Item().Table(tabla =>
		 {
			 tabla.ColumnsDefinition(columns =>
			 {
				 columns.RelativeColumn(6);
				 columns.RelativeColumn(4);
				 columns.RelativeColumn(2);

			 });

			 tabla.Header(header =>
			 {
				 header.Cell().Background("#257272")
				 .Padding(2).Text("Detalles presupuesto").FontColor("#fff");

				 header.Cell().Background("#257272")
				 .Padding(2).Text("Fecha aproximada de entrega").FontColor("#fff");

				 header.Cell().Background("#257272")
				 .Padding(2).Text("Costo").FontColor("#fff");



			 });




			 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
			 .Padding(2).Text(DescripcionPresupuesto).FontSize(10);

			 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
			 .Padding(2).Text(FechaPromesaEntrega.ToString("dd-MM-yyyy")).FontSize(10);

			 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
			 .Padding(2).Text(ManoDeObra.ToString()).FontSize(10);


		 });
         col1.Item().LineHorizontal(0.5f);
         if (1 == 1)
			 col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
			 .Column(column =>
			 {
				 column.Item().Text("Politicas").FontSize(14);
				 //aca va algun atributo de empresa con sus politicas
				 column.Item().Text(emp.PoliticasEmpresa);
				 column.Spacing(5);
			 });

		 col1.Spacing(10);
	 });

	 page.Footer()
	 .AlignRight()
	 .Text(txt =>
	 {
		 txt.Span("Pagina ").FontSize(10);
		 txt.CurrentPageNumber().FontSize(10);
		 txt.Span(" de ").FontSize(10);
		 txt.TotalPages().FontSize(10);
	 });
 });
}).GeneratePdf();
			
			return data;



		}
        public byte[] GenerarPdfOrdenServicioEntregada( Empresa emp)
        {
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", "Empresa.png");
            byte[] imageData = File.ReadAllBytes(imagePath);
            var data =Document.Create(document =>
			{
				document.Page(page =>
				{
					page.Margin(30);

					page.Header().ShowOnce().Row(row =>
					{
						row.ConstantItem(150).Image(imageData);

						row.RelativeItem().Column(col =>
						{ // estos van a ser datos de la empresa. 
							col.Item().AlignCenter().Text(emp.Nombre.ToUpper()).Bold().FontSize(14);
							col.Item().AlignCenter().Text(emp.Direccion).FontSize(9);
							col.Item().AlignCenter().Text(emp.Telefono).FontSize(9);
							col.Item().AlignCenter().Text(emp.Email).FontSize(9);
						});

						row.RelativeItem().Column(col =>
						{
							col.Item().Border(1).BorderColor("#257272")
							.AlignCenter().Text("Numero de Orden");
							//ID
							col.Item().Background("#257272").Border(1)
							.BorderColor("#257272").AlignCenter()
							.Text(Id.ToString()).FontColor("#fff");
							//FECHA DE INGRESO
							col.Item().Border(1).BorderColor("#257272")
							.AlignCenter().Text(Fecha.ToString("dd-MM-yyyy"));
						});
					});

					page.Content().PaddingVertical(10).Column(col1 =>
					{ //DATOS DEL CLIENTE
						col1.Item().Column(col2 =>
						{
							col2.Item().Text("Datos del cliente").Underline().Bold();

							col2.Item().Text(txt =>
							{
								txt.Span("Nombre: ").SemiBold().FontSize(10);
								txt.Span(Cliente.Nombre + " " + Cliente.Apellido).FontSize(10);
							});

							col2.Item().Text(txt =>
							{
								txt.Span("Celular/Telefono: ").SemiBold().FontSize(10);
								txt.Span(Cliente.Telefono).FontSize(10);
							});

							col2.Item().Text(txt =>
							{
								txt.Span("Email: ").SemiBold().FontSize(10);
								txt.Span(Cliente.Email.Value).FontSize(10);
							});
							col2.Item().Text(txt =>
							{
								txt.Span("Direccion: ").SemiBold().FontSize(10);
								txt.Span(Cliente.Direccion).FontSize(10);
							});
						});

						col1.Item().LineHorizontal(0.5f);

						col1.Item().Table(tabla =>
						{
							tabla.ColumnsDefinition(columns =>
							{
								columns.RelativeColumn(3);
								columns.RelativeColumn(3);
								columns.RelativeColumn(5);
								columns.RelativeColumn(2);


							});

							tabla.Header(header =>
							{
								header.Cell().Background("#257272")
								.Padding(2).Text("Producto").FontColor("#fff");

								header.Cell().Background("#257272")
								.Padding(2).Text("Numero de serie").FontColor("#fff");

								header.Cell().Background("#257272")
								.Padding(2).Text("Descripcion problema").FontColor("#fff");
								header.Cell().Background("#257272")
							   .Padding(2).Text("Tecnico").FontColor("#fff");

							});




							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).Text(Producto.Marca + " " + Producto.Modelo + " " + Producto.Version).FontSize(10);

                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(NumeroSerie).FontSize(10);

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(Descripcion).FontSize(10);

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(Tecnico.Nombre).FontSize(10);



						});
						col1.Item().LineHorizontal(0.5f);

						col1.Item().Table(tabla =>
						{
							tabla.ColumnsDefinition(columns =>
							{
								columns.RelativeColumn(6);
								columns.RelativeColumn(4);
								columns.RelativeColumn(2);

							});

							tabla.Header(header =>
							{
								header.Cell().Background("#257272")
								.Padding(2).Text("Detalles presupuesto").FontColor("#fff");

								header.Cell().Background("#257272")
								.Padding(2).Text("Fecha aproximada de entrega").FontColor("#fff");

								header.Cell().Background("#257272")
								.Padding(2).Text("Costo").FontColor("#fff");



							});




							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(DescripcionPresupuesto).FontSize(10);

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(FechaPromesaEntrega.ToString("dd-MM-yyyy")).FontSize(10);

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(ManoDeObra.ToString()).FontSize(10);


						});
						col1.Item().LineHorizontal(0.5f);

						col1.Item().Table(tabla =>
						{
							tabla.ColumnsDefinition(columns =>
							{
								columns.RelativeColumn(3);
								columns.RelativeColumn();
								columns.RelativeColumn();

							});

							tabla.Header(header =>
							{
								header.Cell().Background("#257272")
								.Padding(2).Text("Estado de la reparacion").FontColor("#fff");

								header.Cell().Background("#257272")
								.Padding(2).Text("Fecha de entrega").FontColor("#fff");

								header.Cell().Background("#257272")
								.Padding(2).Text("Costo final").FontColor("#fff");



							});


							var estadoreparacion = "Reparada";

							if (!Reparada)
							{
								estadoreparacion = "No reparada";

							}

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(estadoreparacion).FontSize(10);

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(FechaEntrega.ToString("dd-MM-yyyy")).FontSize(10);

							tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
							.Padding(2).Text(CostoFinal.ToString()).FontSize(10);


						});



						if (1 == 1)
							col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
							.Column(column =>
							{
								column.Item().Text("Politicas").FontSize(14);
								//aca va algun atributo de empresa con sus politicas
								column.Item().Text(emp.PoliticasEmpresa);
								column.Spacing(5);
							});

						col1.Spacing(10);
					});

					page.Footer()
					.AlignRight()
					.Text(txt =>
					{
						txt.Span("Pagina ").FontSize(10);
						txt.CurrentPageNumber().FontSize(10);
						txt.Span(" de ").FontSize(10);
						txt.TotalPages().FontSize(10);
					});
				});
			}).GeneratePdf();
           
            return data;



        }



    }










}
