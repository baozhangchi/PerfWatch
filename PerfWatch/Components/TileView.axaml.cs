using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
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

    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
        AvaloniaProperty.Register<TileView, IEnumerable>(
            nameof(ItemsSource));

    public static readonly StyledProperty<object?> ActivatedItemProperty = AvaloniaProperty.Register<TileView, object?>(
        nameof(ActivatedItem));

    private WrapPanel? _itemsHost;

    public ObservableCollection<object> ActivatedItems { get; } = new ObservableCollection<object>();

    private readonly IDictionary<object, TileItem> _items = new Dictionary<object, TileItem>();

    public TileView()
    {
        Items.CollectionChanged += Items_CollectionChanged;

        ActivatedItems.CollectionChanged += ActivatedItems_CollectionChanged;
    }

    private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var item in e.NewItems!)
                {
                    ActivatedItems.Add(item);
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (var item in e.OldItems!)
                {
                    ActivatedItems.Remove(item);
                }
                break;

            case NotifyCollectionChangedAction.Reset:
                ActivatedItems.Clear();
                break;
        }
    }

    private void ActivatedItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_itemsHost == null)
        {
            return;
        }
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var item in e.NewItems!)
                {
                    var tileItem = new TileItem() { Content = item };
                    tileItem.Click += TileItem_Click;
                    _items.Add(item, tileItem);
                    _itemsHost?.Children.Add(tileItem);
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (var item in e.OldItems!)
                {
                    if (_items.TryGetValue(item, out var tileItem))
                    {
                        tileItem.Click -= TileItem_Click;
                        _items.Remove(item);
                        _itemsHost?.Children.Remove(tileItem);
                    }
                }
                break;

            case NotifyCollectionChangedAction.Reset:
                _items.Clear();
                _itemsHost?.Children.Clear();
                break;
        }
    }

    private void TileItem_Click(object? sender, EventArgs e)
    {
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
        _itemsHost = e.NameScope.Find<WrapPanel>("PART_ItemsPanel");
        if (_itemsHost != null)
        {
            _itemsHost.Children.Clear();
            _items.Clear();
            if (ActivatedItems.Any())
            {
                foreach (var item in ActivatedItems)
                {
                    var tileItem = new TileItem() { Content = item };
                    tileItem.Click += TileItem_Click;
                    _items.Add(item, tileItem);
                    _itemsHost?.Children.Add(tileItem);
                }
            }
        }
    }
}