using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;

namespace PerfWatch.Components;

public class TileView : TemplatedControl
{
    public static readonly StyledProperty<int> RowsProperty = AvaloniaProperty.Register<TileView, int>(
        nameof(Rows));

    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<TileView, int>(
        nameof(Columns));

    public static readonly StyledProperty<double> ItemWidthProperty = AvaloniaProperty.Register<TileView, double>(
        nameof(ItemWidth));

    public static readonly StyledProperty<double> ItemHeightProperty = AvaloniaProperty.Register<TileView, double>(
        nameof(ItemHeight));

    public static readonly StyledProperty<IEnumerable> ActivatedItemsProperty =
        AvaloniaProperty.Register<TileView, IEnumerable>(
            nameof(ActivatedItems));

    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
        AvaloniaProperty.Register<TileView, IEnumerable>(
            nameof(ItemsSource));

    public static readonly StyledProperty<object?> ActivatedItemProperty = AvaloniaProperty.Register<TileView, object?>(
        nameof(ActivatedItem));

    public TileView()
    {
        Items.CollectionChanged += (sender, e) =>
        {
            if (ActivatedItem != null && Items.Contains(ActivatedItem))
            {
                ActivatedItems = Items.Where(x => x != ActivatedItem).ToList();
            }
            else
            {
                ActivatedItems = Items;
                ActivatedItem = null;
            }
        };
    }

    protected override Type StyleKeyOverride { get; } = typeof(TileView);

    public int Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public double ItemWidth
    {
        get => GetValue(ItemWidthProperty);
        set => SetValue(ItemWidthProperty, value);
    }

    public double ItemHeight
    {
        get => GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    public IEnumerable ActivatedItems
    {
        get => GetValue(ActivatedItemsProperty);
        set => SetValue(ActivatedItemsProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    [Content] public ObservableCollection<object> Items { get; } = new();

    public object? ActivatedItem
    {
        get => GetValue(ActivatedItemProperty);
        set => SetValue(ActivatedItemProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        switch (change.Property.Name)
        {
            case nameof(Rows):
            case nameof(Columns):
                UpdateItemSize();
                break;
        }
    }

    private void UpdateItemSize()
    {
        var rows = Rows == 0 ? 1 : Rows;
        var columns = Columns == 0 ? 1 : Columns;
        ItemWidth = Math.Max(0, Bounds.Width / columns);
        ItemHeight = Math.Max(0, Bounds.Height / rows);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        UpdateItemSize();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        ActivatedItems = Items;
    }
}